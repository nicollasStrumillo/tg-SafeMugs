using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Produto;

namespace backend.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet("lista")]
    public async Task<ActionResult<IReadOnlyList<ProdutoCompletoDTO>>> GetLista()
    {
        var produtos = await _produtoService.ObterTodosAsync();
        return Ok(produtos);
    }

    [HttpGet("{produtoId}")]
    public async Task<ActionResult<ProdutoCompletoDTO?>> GetProduto(int produtoId)
    {
        var produto = await _produtoService.ObterProdutoCompletoPorIdAsync(produtoId);
        if (produto == null)
            return NotFound();
        return Ok(produto);
    }

    // Pega Nome do produto por query string:
    [HttpGet("por-nome")]
    public async Task<ActionResult<ProdutoCompletoDTO?>> GetProdutoPorNome([FromQuery] string nome)
    {
        var produto = await _produtoService.ObterProdutoCompletoPorNomeAsync(nome);
        if (produto == null)
            return NotFound();
        return Ok(produto);
    }

    [HttpGet("comentarios/{produtoId}")]
    public async Task<ActionResult<List<ComentarioProdutoDTO>>> GetComentarios(int produtoId)
    {
        var comentarios = await _produtoService.ObterComentariosPorProdutoIdAsync(produtoId);
        return Ok(comentarios);
    }

    [HttpPost("comentarios/{produtoId}")]
    public async Task<IActionResult> FazerComentario(int produtoId, [FromBody] ComentarioRequest request)
    {
        await _produtoService.FazerComentarioAsync(produtoId, request.NomeCompleto, request.Comentario);
        return Ok(new { mensagem = "Comentário realizado com sucesso."});
    }

    [HttpPatch("comentarios")]
    public async Task<IActionResult> AtualizarComentario([FromBody] EdicaoComentarioRequest request)
    {
        await _produtoService.AtualizarComentarioAsync(request.ComentarioId, request.Comentario);
        return Ok(new { mensagem = "Comentário atualizado com sucesso."});
    }
}
