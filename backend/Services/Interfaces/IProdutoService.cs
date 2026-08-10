using backend.DTOs.Produto;

namespace backend.Services.Interfaces;

public interface IProdutoService
{
    Task<IReadOnlyList<ProdutoCompletoDTO>> ObterTodosAsync();
    Task<ProdutoCompletoDTO?> ObterProdutoCompletoPorIdAsync(int produtoId);
    Task<ProdutoCompletoDTO?> ObterProdutoCompletoPorNomeAsync(string nome);
    Task<List<ComentarioProdutoDTO>> ObterComentariosPorProdutoIdAsync(int produtoId);
    Task FazerComentarioAsync(int produtoId, string? nomeCompleto, string comentario);
    Task AtualizarComentarioAsync(int comentarioId, string comentario);
}
