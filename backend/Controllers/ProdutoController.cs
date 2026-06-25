using backend.models;
using backend.services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

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
    public async Task<ActionResult<IReadOnlyList<Produto>>> GetLista(CancellationToken cancellationToken)
    {
        var produtos = await _produtoService.ObterTodosAsync(cancellationToken);
        return Ok(produtos);
    }
}
