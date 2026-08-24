using backend.Data;
using backend.DTOs.Pedido;
using backend.models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Pedido>> ObterPedidosPorUsuarioIdAsync(int usuarioId)
    {
        return await _context.Pedidos
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<Pedido?> ObterPedidoPorIdAsync(int pedidoId)
    {
        return await _context.Pedidos
            .Include(p => p.Usuario)
                .ThenInclude(u => u.Perfil)
            .Include(p => p.Usuario)
                .ThenInclude(u => u.Endereco)    
            .Include(p => p.Endereco)
            .Include(p => p.Carrinho)
                .ThenInclude(c => c.Itens)
                    .ThenInclude(ic => ic.Produto)
            .FirstOrDefaultAsync(p => p.Id == pedidoId);
    }

    public async Task<List<PedidoResumoDTO>> ObterPedidoPorNumeroAsync(string numeroPedido, int usuarioId)
    {
        var sql = $@"SELECT Id, NumeroPedido, ValorTotal, QuantidadeItens, DataPedido, UsuarioId
                    FROM pedidos
                    WHERE NumeroPedido LIKE '%{numeroPedido}%' AND UsuarioId = {usuarioId}";

        try
        {
            return await _context.Database
                .SqlQueryRaw<PedidoResumoDTO>(sql)
                .ToListAsync();
        }
        catch (MySqlConnector.MySqlException ex)
        {
            throw new InvalidOperationException(
                $"MYSQL_ERROR: {ex.Message}, on query: {sql}", ex);
        }
    }

    public async Task CancelarPedidoAsync(Pedido pedido)
    {
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
    }
}
