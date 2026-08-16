using backend.DTOs.Pedido;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/pedido")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [Authorize]
    [HttpPost("criar/{usuarioId}")]
    public async Task<ActionResult<PedidoDTO>> CriarPedido(int usuarioId)
    {
        var pedido = await _pedidoService.CriarPedido(usuarioId);
        return Ok(pedido);
    }
}
