using backend.DTOs.Produto;
using backend.Exceptions;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/administracao")]
[Authorize(Roles = "Administrador")]
public class AdministracaoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly IReposicaoEstoqueService _reposicaoEstoqueService;

    public AdministracaoController(IProdutoService produtoService, IReposicaoEstoqueService reposicaoEstoqueService)
    {
        _produtoService = produtoService;
        _reposicaoEstoqueService = reposicaoEstoqueService;
    }

    [HttpGet("avaliacoes")]
    public async Task<ActionResult<List<AvaliacaoAdminDTO>>> ObterAvaliacoes()
    {
        var avaliacoes = await _produtoService.ObterTodasAvaliacoesAsync();
        return Ok(avaliacoes);
    }

    [HttpDelete("avaliacoes/{avaliacaoId}")]
    public async Task<IActionResult> ExcluirAvaliacao(int avaliacaoId)
    {
        await _produtoService.ExcluirAvaliacaoAsync(avaliacaoId);
        return Ok(new { mensagem = "Avaliação excluída com sucesso." });
    }

    [HttpPost("reposicao-estoque")]
    public async Task<ActionResult<ReposicaoEstoqueResultadoDTO>> ImportarReposicao(IFormFile arquivoXml)
    {
        if (arquivoXml is null || arquivoXml.Length == 0)
            throw new ValidationException("O arquivo XML está vazio ou não foi recebido.");

        await using var stream = arquivoXml.OpenReadStream();
        var resultado = await _reposicaoEstoqueService.ImportarAsync(stream);
        return Ok(resultado);
    }
}
