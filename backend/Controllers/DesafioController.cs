using backend.DTOs.Desafio;
using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/desafios")]
public class DesafioController : ControllerBase
{
    private readonly IDesafioService _desafioService;
    private readonly IDesafiosBackupService _desafiosBackupService;

    public DesafioController(IDesafioService desafioService, IDesafiosBackupService desafiosBackupService)
    {
        _desafioService = desafioService;
        _desafiosBackupService = desafiosBackupService;
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

    [HttpGet("{nomeDesafio}")]
    public async Task<ActionResult<DesafioResponse?>> ObterPorNome(string nomeDesafio)
    {
        var desafio = await _desafioService.ObterPorNomeAsync(nomeDesafio);
        if (desafio == null) return NotFound();

        return Ok(desafio);
    }

    [HttpGet("detalhes/{id}")]
    public async Task<ActionResult<DesafioDetalhesResponse?>> ObterDesafioDetalhesPorId(int id)
    {
        var desafio = await _desafioService.ObterDesafioDetalhesPorId(id);
        if (desafio == null) return NotFound();

        return Ok(desafio);
    }

    [HttpPost("resolver-quiz/{id}")]
    public async Task<ActionResult<ResolverQuizDesafioResponse>> ResolverQuizDesafioAsync(int id, [FromBody] int[] linhasSelecionadas)
    {
        var resposta = await _desafioService.TrySolveQuizDesafioAsync(id, linhasSelecionadas);

        return Ok(new ResolverQuizDesafioResponse
        {
            Sucesso = resposta.sucesso,
            Mensagem = resposta.mensagem
        });
    }

    [HttpGet("backup")]
    public async Task<ActionResult<string?>> BackupDesafiosGenerateAsync()
    {
        var backupCode = await _desafiosBackupService.BackupDesafiosGenerateAsync();

        return Ok(backupCode);
    }

    [HttpPost("restore")]
    public async Task<ActionResult<int>> RestoreDesafiosAsync([FromBody] RestoreDesafiosRequest request)
    {
        var restoredCount = await _desafiosBackupService.RestoreAsync(request.BackupDesafios);
        return Ok(restoredCount);
    }
}
