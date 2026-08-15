using backend.DTOs.Desafio;
using backend.models;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;
using backend.Services.Interfaces.Util.QuizDesafios;
using backend.Services.Implementations.Util.QuizDesafios;

namespace backend.Services.Implementations;

public class DesafioService : IDesafioService
{
    private readonly IDesafioRepository _desafioRepository;

    private readonly INotificationService _notificationService;
    private readonly IQuizDesafioService _quizDesafioService;

    public DesafioService(IDesafioRepository desafioRepository, INotificationService notificationService, IQuizDesafioService quizDesafioService)
    {
        _desafioRepository = desafioRepository;
        _notificationService = notificationService;
        _quizDesafioService = quizDesafioService;
    }

    public async Task<IEnumerable<DesafioResponse>> ObterTodosAsync(bool resolverScoreBoard = true)
    {
        var desafios = await _desafioRepository.ObterTodosAsync();

        if (resolverScoreBoard) await SolveIfAsync(DesafiosEnum.EncontrarScoreBoard, () => true);
        
        return desafios.Select(d => MapToDesafioResponse(d));
    }

    private DesafioResponse MapToDesafioResponse(Desafio desafio, bool isRestored = false)
    {
        QuizDesafio? quiz = null;
        if (EnumExtensions.TryGetEnumByNomeDisplay<DesafiosEnum>(desafio.Nome, out var enumDesafio))
            quiz = _quizDesafioService.GetQuizDesafio(enumDesafio);
        
        return new DesafioResponse
        {
            Id = desafio.Id,
            Nome = desafio.Nome,
            Descricao = desafio.Descricao,
            Categoria = desafio.Categoria.GetNomeDisplay(),
            Dificuldade = desafio.Dificuldade,
            Resolvido = desafio.Resolvido,
            IsRestored = isRestored,

            PossuiQuiz = quiz != null,
            QuizResolvido = quiz?.Resolvido ?? false,

            DicasDesafio = desafio.DicasDesafio.Select(di => new DicaDesafioDTO
            {
                Id = di.Id,
                NrDica = di.NrDica,
                Texto = di.Texto
            }).ToList()
        };
    }

    public IEnumerable<string> ObterCategorias()
    {
        var categorias = Enum.GetValues(typeof(CategoriaDesafio)).Cast<CategoriaDesafio>();
        return categorias.Select(c => c.GetNomeDisplay());
    }

    public async Task<DesafioResponse?> ObterPorNomeAsync(string nomeDesafio)
    {
        var desafio = await _desafioRepository.FindByNameAsync(nomeDesafio);
        if (desafio == null) return null;

        return MapToDesafioResponse(desafio);
    }

    public async Task<DesafioDetalhesResponse?> ObterDesafioDetalhesPorId(int id)
    {
        var desafio = await _desafioRepository.FindByIdAsync(id);
        if (desafio == null) return null;

        QuizDesafio? quiz = null;
        if (EnumExtensions.TryGetEnumByNomeDisplay<DesafiosEnum>(desafio.Nome, out var enumDesafio))
            quiz = _quizDesafioService.GetQuizDesafio(enumDesafio);

        return new DesafioDetalhesResponse
        {
            Id = desafio.Id,
            Nome = desafio.Nome,
            Descricao = desafio.Descricao,
            DescricaoDetalhes = desafio.DescricaoDetalhes,
            Categoria = desafio.Categoria.GetNomeDisplay(),
            DescricaoCategoria = desafio.Categoria.GetDescription(),
            Dificuldade = desafio.Dificuldade,
            Resolvido = desafio.Resolvido,

            PossuiQuiz = quiz != null,
            QuizResolvido = quiz?.Resolvido ?? false,

            DicasDesafio = desafio.DicasDesafio.Select(di => new DicaDesafioDTO
            {
                Id = di.Id,
                NrDica = di.NrDica,
                Texto = di.Texto
            }).ToList(),

            QuizDesafio = quiz == null ? null : new QuizDesafioDTO
            {
                NomeDesafio = quiz.NomeDesafio,
                Linguagem = quiz.Linguagem,
                Resolvido = quiz.Resolvido,
                LinhasQuiz = quiz.LinhasQuiz,
                LinhasCorretas = quiz.Resolvido ? quiz.LinhasCorretas : new List<int>(),
                LinhasCodigoSeguro = quiz.LinhasCodigoSeguro,
                MensagemSeguro = quiz.MensagemSeguro
            }
        };
    }

    // Resolucao de QuizDesafio
    public async Task<(bool sucesso, string mensagem)> TrySolveQuizDesafioAsync(int idDesafio, int[] linhasSelecionadas)
    {
        var desafio = await _desafioRepository.FindByIdAsync(idDesafio);
        if (desafio == null)
            return (false, "Desafio não encontrado.");
        
        if (!EnumExtensions.TryGetEnumByNomeDisplay<DesafiosEnum>(desafio.Nome, out var enumDesafio))
            return (false, "Desafio Enum não encontrado.");
        
        var sucesso = _quizDesafioService.TrySolveQuizDesafio(enumDesafio, linhasSelecionadas, out string msg);
        return (sucesso, msg);
    }

    // Resolucao de Desafios
    public async Task SolveIfAsync(DesafiosEnum desafio, Func<bool> criteria)
    {
        string nomeDesafio = desafio.GetNomeDisplay();

        if (!criteria()) return;

        Desafio? _desafio = await _desafioRepository.FindByNameAsync(nomeDesafio);

        if (_desafio != null && !_desafio.Resolvido)
            await SolveAsync(_desafio);
    }

    private async Task SolveAsync(Desafio desafio, bool isRestored = false)
    {
        await _desafioRepository.ResolverDesafioAsync(desafio);

        await _notificationService.NotifyDesafioSolved(MapToDesafioResponse(desafio, isRestored));
    }

    public async Task<int> SolveListDesafiosAsync(int[] ids)
    {
        var desafios = await _desafioRepository.FindByIdsAsync(ids);
        int restaurados = 0;

        foreach (var desafio in desafios)
        {
            if (!desafio.Resolvido)
            {
                await SolveAsync(desafio, isRestored: true);
                ++restaurados;
            }
                
        }
        return restaurados;
    }
}
