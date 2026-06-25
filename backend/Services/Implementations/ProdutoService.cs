using backend.models;
using backend.repositories.Interfaces;
using backend.services.Interfaces;

namespace backend.services.Implementations;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return _produtoRepository.ObterTodosAsync(cancellationToken);
    }
}
