using backend.DTOs.Desafio;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/desafios")]
public class DesafioController : ControllerBase
{
    private readonly IDesafioService _desafioService;

    public DesafioController(IDesafioService desafioService)
    {
        _desafioService = desafioService;
    }

    [HttpGet("lista")]
    public async Task<ActionResult<IEnumerable<DesafioResponse>>> ObterTodos()
    {
        var desafios = await _desafioService.ObterTodosAsync();
        return Ok(desafios);
    }

    [HttpGet("categorias")]
    public ActionResult<IEnumerable<string>> ObterCategorias()
    {
        var categorias = _desafioService.ObterCategorias();
        return Ok(categorias);
    }
}
