using backend.models;

namespace backend.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<IReadOnlyList<Produto>> ObterTodosAsync();
    Task<Produto?> ObterProdutoPorNomeAsync(string nome);
    Task<Produto?> ObterProdutoCompletoPorIdAsync(int produtoId);
    Task<List<ComentarioProduto>> ObterComentariosPorProdutoIdAsync(int produtoId);
    Task FazerComentarioAsync(int produtoId, int? usuarioId, string comentario);
    Task AtualizarComentarioAsync(ComentarioProduto comentario, string novaDescricao);
    Task<ComentarioProduto?> ProcurarComentarioPorIdAsync(int comentarioId);
}
