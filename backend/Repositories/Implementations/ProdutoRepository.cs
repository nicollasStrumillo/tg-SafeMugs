using backend.Data;
using backend.models;
using backend.repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.repositories.Implementations;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDBContext _dbContext;

    public ProdutoRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Produtos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
