using backend.DTOs.Pedido;

namespace backend.Services.Interfaces;

public interface IPedidoService
{
   Task<PedidoDTO> CriarPedido(int usuarioId);
}
