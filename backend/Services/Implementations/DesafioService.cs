using backend.DTOs.Desafio;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class DesafioService : IDesafioService
{
    private readonly IDesafioRepository _desafioRepository;

    public DesafioService(IDesafioRepository desafioRepository)
    {
        _desafioRepository = desafioRepository;
    }

    public async Task<IEnumerable<DesafioResponse>> ObterTodosAsync()
    {
        var desafios = await _desafioRepository.ObterTodosAsync();
        return desafios.Select(d => new DesafioResponse
        {
            Id = d.Id,
            Nome = d.Nome,
            Descricao = d.Descricao,
            Categoria = d.Categoria.GetDisplayName(),
            Dificuldade = d.Dificuldade,
            UrlMitigacao = d.UrlMitigacao,
            DicasDesafio = d.DicasDesafio.Select(di => new DicaDesafioDTO
            {
                Id = di.Id,
                NrDica = di.NrDica,
                Texto = di.Texto
            }).ToList()
        });
    }

    public IEnumerable<string> ObterCategorias()
    {
        var categorias = Enum.GetValues(typeof(CategoriaDesafio)).Cast<CategoriaDesafio>();
        return categorias.Select(c => c.GetDisplayName());
    }
}
