using backend.Exceptions;
using backend.models.Enums;
using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;
using backend.Services.Interfaces.Util.QuizDesafios;
using HashidsNet;
using System.Text.RegularExpressions;

namespace backend.Services.Implementations.Util;
    
public class DesafiosBackupService : IDesafiosBackupService
{
    private readonly IDesafioService _desafioService;
    private readonly IQuizDesafioService _quizService;
    private readonly Hashids _hashids;
    private static readonly Regex backupDesafiosRegex = new("^[a-zA-Z0-9]+$", RegexOptions.Compiled);

    public DesafiosBackupService(IDesafioService desafioService, IQuizDesafioService quizService)
    {
        _desafioService = desafioService;
        _quizService = quizService;
        _hashids = new Hashids(
            salt: "safemugs salt",
            minHashLength: 60,
            alphabet: "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");
    }

    public async Task<string?> BackupDesafiosGenerateAsync()
    {   
        var desafios = await _desafioService.ObterTodosAsync(resolverScoreBoard: false);
        var ids = desafios.Where(d => d.Resolvido).Select(d => d.Id).ToArray();

        if (ids.Length == 0)
            return null;

        return _hashids.Encode(ids);
    }

    public async Task<int> RestoreAsync(string backupDesafios)
    {
        if (!backupDesafiosRegex.IsMatch(backupDesafios)) 
            throw new ValidationException("String de backup inválida.");

        int[] ids = _hashids.Decode(backupDesafios);

        if (ids.Length == 0)
            throw new ValidationException("String de backup inválida.");

        return await _desafioService.SolveListDesafiosAsync(ids);
    }

    public string? BackupQuizzesGenerateAsync()
    {   
        var quizzes = _quizService.GetAllQuizDesafio();
        var ids = quizzes.Where(q => q.Value.Resolvido).Select(u => (int)u.Key).ToArray();

        if (ids.Length == 0)
            return null;

        return _hashids.Encode(ids);
    }

    public int RestoreQuizzesAsync(string backupQuizzes)
    {
        if (!backupDesafiosRegex.IsMatch(backupQuizzes)) 
            throw new ValidationException("String de backup inválida.");

        int[] ids = _hashids.Decode(backupQuizzes);

        if (ids.Length == 0)
            throw new ValidationException("String de backup inválida.");

        int count = 0;
        foreach (var id in ids)
        {
            if (Enum.IsDefined(typeof(DesafiosEnum), id))
            {
                var desafioEnum = (DesafiosEnum)id;
                if (_quizService.TrySolveQuizDesafio(desafioEnum, [], out _, isRestore: true))
                {
                    count++;
                }
            }
        }

        return count;
    }

}
