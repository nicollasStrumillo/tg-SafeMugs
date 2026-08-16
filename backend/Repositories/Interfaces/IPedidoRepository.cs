using backend.models;

namespace backend.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido> PersistirPedidoAsync(Pedido pedido);
    Task MudarNumeroPedidoAsync(Pedido pedido, string numeroPedido);
}
