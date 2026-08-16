using backend.Data;
using backend.models;
using backend.Repositories.Interfaces;

namespace backend.Repositories.Implementations;

public class PedidoRepository : IPedidoRepository
{
    private readonly ApplicationDBContext _context;

    public PedidoRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Pedido> PersistirPedidoAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task MudarNumeroPedidoAsync(Pedido pedido, string numeroPedido)
    {
        pedido.NumeroPedido = numeroPedido;
        await _context.SaveChangesAsync();
    }
}
