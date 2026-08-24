using backend.DTOs.Pedido;
using backend.models;

namespace backend.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido> PersistirPedidoAsync(Pedido pedido);
    Task MudarNumeroPedidoAsync(Pedido pedido, string numeroPedido);
    Task<List<Pedido>> ObterPedidosPorUsuarioIdAsync(int usuarioId);
    Task<Pedido?> ObterPedidoPorIdAsync(int pedidoId);
    Task<List<PedidoResumoDTO>> ObterPedidoPorNumeroAsync(string numeroPedido, int usuarioId);
    Task CancelarPedidoAsync(Pedido pedido);
}
