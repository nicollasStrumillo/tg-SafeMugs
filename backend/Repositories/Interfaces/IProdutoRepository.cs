using backend.models;

namespace backend.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<List<ComentarioProduto>> ObterComentariosPorProdutoIdAsync(int produtoId);
    Task FazerComentarioAsync(int produtoId, int? usuarioId, string comentario);
}
