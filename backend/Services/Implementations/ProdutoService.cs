using backend.models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _produtoRepository.ObterTodosAsync(cancellationToken);
    }

    public async Task<List<ComentarioProduto>> ObterComentariosPorProdutoIdAsync(int produtoId)
    {
        return await _produtoRepository.ObterComentariosPorProdutoIdAsync(produtoId);
    }

    public async Task FazerComentarioAsync(int produtoId, int? usuarioId, string comentario)
    {
        await _produtoRepository.FazerComentarioAsync(produtoId, usuarioId, comentario);
    }
}
