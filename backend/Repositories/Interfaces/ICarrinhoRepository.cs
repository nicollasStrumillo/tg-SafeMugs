using backend.models;

namespace backend.Repositories.Interfaces;

public interface ICarrinhoRepository
{
    Task<Carrinho?> ObterCarrinhoAsNoTrackingPorUsuarioIdAsync(int usuarioId);
    Task<Carrinho> ObterOuCriarCarrinhoAtivoAsync(int usuarioId);
    Task AdicionarUnidadeProdutoAoCarrinhoAsync(Carrinho carrinho, Produto produto, int quantidade);
    Task RemoverUnidadeProdutoDoCarrinhoAsync(Carrinho carrinho, int produtoId, int quantidade);
    Task FinalizarCarrinhoAsync(Carrinho carrinho);
}
