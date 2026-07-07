using backend.Data;
using backend.models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementations;

public class DesafioRepository : IDesafioRepository
{
    private readonly ApplicationDBContext _dbContext;

    public DesafioRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Desafio>> ObterTodosAsync()
    {
        var desafios = await _dbContext.Desafios
            .Include(d => d.DicasDesafio)
            .Select(d => new Desafio
            {
                Id = d.Id,
                Nome = d.Nome,
                Descricao = d.Descricao,
                Categoria = d.Categoria,
                Dificuldade = d.Dificuldade,
                UrlMitigacao = d.UrlMitigacao,
                DicasDesafio = d.DicasDesafio.Select(dd => new DicaDesafio
                {
                    Id = dd.Id,
                    NrDica = dd.NrDica,
                    Texto = dd.Texto,
                    DesafioId = dd.DesafioId
                }).ToList()
            })
            .ToListAsync();

        return desafios;
    }
}
