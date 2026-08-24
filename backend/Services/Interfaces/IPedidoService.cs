using backend.DTOs.Pedido;

namespace backend.Services.Interfaces;

public interface IPedidoService
{
   Task<PedidoDTO> CriarPedido(int usuarioId);
   Task<List<PedidoResumoDTO>> ListarPedidosPorUsuarioAsync(int usuarioId);
   Task<PedidoDTO> ObterDetalhesPedidoAsync(int pedidoId);
   Task<List<PedidoResumoDTO>> PesquisarPedidoPorNumeroAsync(string numeroPedido);
   Task CancelarPedidoAsync(int pedidoId);
}
