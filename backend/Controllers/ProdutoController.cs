using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Produto;
using Microsoft.AspNetCore.Authorization;
using backend.Authentication.Interfaces;
using backend.models.Enums;

namespace backend.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly IDesafioService _desafioService;
    private readonly IJwtService _jwtService;

    public ProdutoController(IProdutoService produtoService, IDesafioService desafioService ,IJwtService jwtService)
    {
        _produtoService = produtoService;
        _jwtService = jwtService;
        _desafioService = desafioService;
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
        return Ok(produto);
    }

    // Pega Nome do produto por query string:
    [HttpGet("por-nome")]
    public async Task<ActionResult<ProdutoCompletoDTO?>> GetProdutoPorNome([FromQuery] string nome)
    {
        var produto = await _produtoService.ObterProdutoCompletoPorNomeAsync(nome);
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

    [Authorize]
    [HttpPost("avaliacoes/{produtoId}")]
    public async Task<IActionResult> AvaliarProduto(int produtoId, [FromBody] FazerAvaliacaoRequest request)
    {
        await _produtoService.AvaliarProdutoAsync(produtoId, request.UsuarioId, request.Nota, request.Comentario);
        return Ok(new { mensagem = "Avaliação realizada com sucesso."});
    }

    [HttpGet("avaliacoes")]
    public async Task<ActionResult<List<AvaliacaoAdminDTO>>> ObterAvaliacoes()
    {
        var token = _jwtService.ObterTokenBruto(Request);
        
        if (token is null || !token.Claims.Any(claim =>
            claim.Type == "perfil" &&
            claim.Value.Equals("Administrador", StringComparison.OrdinalIgnoreCase)))
        {
            return Unauthorized("Acesso negado.");
        }

        var avaliacoes = await _produtoService.ObterTodasAvaliacoesAsync();

        await _desafioService.SolveIfAsync(DesafiosEnum.ForjarJwt, () => _jwtService.ValidaAssinatura(token) == false);

        return Ok(avaliacoes);
    }

    [HttpDelete("avaliacoes/{avaliacaoId}")]
    public async Task<IActionResult> ExcluirAvaliacao(int avaliacaoId)
    {
        var token = _jwtService.ObterTokenBruto(Request);
        
        if (token is null || !token.Claims.Any(claim =>
            claim.Type == "perfil" &&
            claim.Value.Equals("Administrador", StringComparison.OrdinalIgnoreCase)))
        {
            return Unauthorized("Acesso negado.");
        }

        await _produtoService.ExcluirAvaliacaoAsync(avaliacaoId);

        await _desafioService.SolveIfAsync(DesafiosEnum.ForjarJwt, () => _jwtService.ValidaAssinatura(token) == false);

        return Ok(new { mensagem = "Avaliação excluída com sucesso." });
    }
}
