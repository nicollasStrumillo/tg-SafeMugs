using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;
using HashidsNet;
using System.Text.RegularExpressions;

namespace backend.Services.Implementations.Util;

public class DesafiosBackupService : IDesafiosBackupService
{
    private readonly IDesafioService _desafioService;
    private readonly Hashids _hashids;
    private static readonly Regex backupDesafiosRegex = new("^[a-zA-Z0-9]+$", RegexOptions.Compiled);

    public DesafiosBackupService(IDesafioService desafioService)
    {
        _desafioService = desafioService;
        _hashids = new Hashids(
            salt: "safemugs salt",
            minHashLength: 60,
            alphabet: "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");
    }

    public async Task<string?> BackupDesafiosGenerateAsync()
    {   
        var desafios = await _desafioService.ObterTodosAsync();
        var ids = desafios.Where(d => d.Resolvido).Select(d => d.Id).ToArray();

        if (ids.Length == 0)
            return null;

        return _hashids.Encode(ids);
    }

    public async Task<int> RestoreAsync(string backupDesafios)
    {
        if (!backupDesafiosRegex.IsMatch(backupDesafios)) 
            throw new ArgumentException("Invalid continue code.");

        int[] ids = _hashids.Decode(backupDesafios);

        if (ids.Length == 0)
            throw new ArgumentException("Invalid continue code.");

        return await _desafioService.SolveListDesafiosAsync(ids);
    }

}
