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

    [Authorize]
    [HttpGet("listar/{usuarioId}")]
    public async Task<ActionResult<List<PedidoResumoDTO>>> ListarPedidosPorUsuario(int usuarioId)
    {
        var pedidos = await _pedidoService.ListarPedidosPorUsuarioAsync(usuarioId);
        return Ok(pedidos);
    }

    [Authorize]
    [HttpGet("detalhes/{pedidoId}")]
    public async Task<ActionResult<PedidoDTO>> ObterDetalhesPedido(int pedidoId)
    {
        var pedido = await _pedidoService.ObterDetalhesPedidoAsync(pedidoId);
        return Ok(pedido);
    }

    [Authorize]
    [HttpGet("pesquisar-pedido/{numeroPedido}")]
    public async Task<ActionResult<List<PedidoResumoDTO>>> PesquisarPedido(string numeroPedido)
    {
        var resultado = await _pedidoService.PesquisarPedidoPorNumeroAsync(numeroPedido);
        return Ok(resultado);
    }

    [Authorize]
    [HttpDelete("cancelar/{pedidoId}")]
    public async Task<ActionResult> CancelarPedido(int pedidoId)
    {
        await _pedidoService.CancelarPedidoAsync(pedidoId);
        return Ok();
    }
}
