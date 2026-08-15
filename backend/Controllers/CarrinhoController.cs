using backend.DTOs.Carrinho;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/carrinho")]
public class CarrinhoController : ControllerBase
{
    private readonly ICarrinhoService _carrinhoService;

    public CarrinhoController(ICarrinhoService carrinhoService)
    {
        _carrinhoService = carrinhoService;
    }

    [HttpGet("{usuarioId}")]
    public async Task<ActionResult<CarrinhoDTO>> ObterOuCriarCarrinhoAtivo(int usuarioId)
    {
        var carrinho = await _carrinhoService.ObterOuCriarCarrinhoAtivoAsync(usuarioId);
        return Ok(carrinho);
    }

    [HttpPatch("adicionar")]
    public async Task<IActionResult> AdicionarUnidadeProdutoAoCarrinho(AdicionarProdutoCarrinhoRequest request)
    {
        await _carrinhoService.AdicionarUnidadeProdutoAoCarrinhoAsync(request.UsuarioId, request.ProdutoId, request.Quantidade);
        return Ok(new { mensagem = "Produto adicionado ao carrinho com sucesso." });
    }

    [HttpPatch("remover")]
    public async Task<IActionResult> RemoverUnidadeProdutoDoCarrinho(RemoverProdutoCarrinhoRequest request)
    {
        await _carrinhoService.RemoverUnidadeProdutoDoCarrinhoAsync(request.UsuarioId, request.ProdutoId, request.Quantidade);
        return Ok(new { mensagem = "Produto removido do carrinho com sucesso." });
    }
}
