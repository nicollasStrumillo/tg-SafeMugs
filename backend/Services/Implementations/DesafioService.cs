using backend.DTOs.Desafio;
using backend.models;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;

namespace backend.Services.Implementations;

public class DesafioService : IDesafioService
{
    private readonly IDesafioRepository _desafioRepository;

    private readonly INotificationService _notificationService;

    public DesafioService(IDesafioRepository desafioRepository, INotificationService notificationService)
    {
        _desafioRepository = desafioRepository;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<DesafioResponse>> ObterTodosAsync(bool resolverScoreBoard = true)
    {
        var desafios = await _desafioRepository.ObterTodosAsync();

        if (resolverScoreBoard) await SolveIfAsync("Encontrar a Score-Board", () => true);
        
        return desafios.Select(d => MapToDesafioResponse(d));
    }

    private DesafioResponse MapToDesafioResponse(Desafio desafio, bool isRestored = false)
    {
        return new DesafioResponse
        {
            Id = desafio.Id,
            Nome = desafio.Nome,
            Descricao = desafio.Descricao,
            Categoria = desafio.Categoria.GetNomeDisplay(),
            Dificuldade = desafio.Dificuldade,
            UrlMitigacao = desafio.UrlMitigacao,
            Resolvido = desafio.Resolvido,
            IsRestored = isRestored,
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

    // Resolucao de Desafios
    public async Task SolveIfAsync(string nomeDesafio, Func<bool> criteria)
    {
        if (!criteria()) return;

        Desafio? desafio = await _desafioRepository.FindByNameAsync(nomeDesafio);

        if (desafio != null && !desafio.Resolvido)
            await SolveAsync(desafio);
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
