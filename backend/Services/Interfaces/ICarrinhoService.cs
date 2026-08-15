using backend.DTOs.Carrinho;

namespace backend.Services.Interfaces;
public interface ICarrinhoService
{
    Task<CarrinhoDTO> ObterOuCriarCarrinhoAtivoAsync(int usuarioId);
    Task AdicionarUnidadeProdutoAoCarrinhoAsync(int usuarioId, int produtoId, int quantidade);
    Task RemoverUnidadeProdutoDoCarrinhoAsync(int usuarioId, int produtoId, int quantidade);
}
