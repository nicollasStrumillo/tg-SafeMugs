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

    public async Task<IEnumerable<DesafioResponse>> ObterTodosAsync()
    {
        var desafios = await _desafioRepository.ObterTodosAsync();

        await SolveIfAsync("Encontrar a score-Board", () => true);
        
        return desafios.Select(d => MapToDesafioResponse(d));
    }

    private DesafioResponse MapToDesafioResponse(Desafio desafio)
    {
        return new DesafioResponse
        {
            Id = desafio.Id,
            Nome = desafio.Nome,
            Descricao = desafio.Descricao,
            Categoria = desafio.Categoria.GetDisplayName(),
            Dificuldade = desafio.Dificuldade,
            UrlMitigacao = desafio.UrlMitigacao,
            Resolvido = desafio.Resolvido,
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
        return categorias.Select(c => c.GetDisplayName());
    }

    // Resolucao de Desafios
    public async Task SolveIfAsync(string nomeDesafio, Func<bool> criteria)
    {
        if (!criteria()) return;

        Desafio? desafio = await _desafioRepository.FindByNameAsync(nomeDesafio);

        if (desafio != null && !desafio.Resolvido)
            await SolveAsync(desafio);
    }

    private async Task SolveAsync(Desafio desafio)
    {
        await _desafioRepository.ResolverDesafioAsync(desafio);

        await _notificationService.NotifyDesafioSolved(MapToDesafioResponse(desafio));
    }
}
