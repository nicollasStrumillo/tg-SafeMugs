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
                Resolvido = d.Resolvido,
                DicasDesafio = d.DicasDesafio.Select(dd => new DicaDesafio
                {
                    Id = dd.Id,
                    NrDica = dd.NrDica,
                    Texto = dd.Texto,
                    DesafioId = dd.DesafioId
                }).ToList()
            })
            .OrderBy(d => d.Dificuldade)
            .ToListAsync();

        return desafios;
    }

    public async Task<Desafio?> FindByNameAsync(string nomeDesafio)
    {
        return await _dbContext.Desafios
            .Include(d => d.DicasDesafio)
            .FirstOrDefaultAsync(d => d.Nome == nomeDesafio);
    }

    public async Task<Desafio?> FindByIdAsync(int id)
    {
        return await _dbContext.Desafios
            .Include(d => d.DicasDesafio)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<Desafio>> FindByIdsAsync(int[] ids)
    {
        return await _dbContext.Desafios
            .Where(d => ids.Contains(d.Id))
            .ToListAsync();
    }

    public async Task ResolverDesafioAsync(Desafio desafio)
    {
        desafio.Resolvido = true;
        await _dbContext.SaveChangesAsync();
    }

    public async Task AtualizarDesafioAsync(Desafio desafio)
    {
        _dbContext.Desafios.Update(desafio);
        await _dbContext.SaveChangesAsync();
    }
}
