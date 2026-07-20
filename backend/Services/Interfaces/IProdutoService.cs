using backend.models;

namespace backend.Services.Interfaces;

public interface IProdutoService
{
    Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<List<ComentarioProduto>> ObterComentariosPorProdutoIdAsync(int produtoId);
    Task FazerComentarioAsync(int produtoId, string? nomeCompleto, string comentario);
    Task AtualizarComentarioAsync(int comentarioId, string comentario);
}
