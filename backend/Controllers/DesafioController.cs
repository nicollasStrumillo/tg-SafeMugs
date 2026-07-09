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

    [HttpGet("backup")]
    public async Task<ActionResult<string?>> BackupDesafiosGenerateAsync()
    {
        try
        {
            var backupCode = await _desafiosBackupService.BackupDesafiosGenerateAsync();

            return Ok(backupCode);
        }
        catch (Exception ex)
        {
            return BadRequest("Não foi possível gerar o backup dos desafios. Erro: " + ex.Message);
        }
    }

    [HttpPost("restore")]
    public async Task<ActionResult<int>> RestoreDesafiosAsync([FromBody] RestoreDesafiosRequest request)
    {
        try
        {
            var restoredCount = await _desafiosBackupService.RestoreAsync(request.BackupDesafios);
            return Ok(restoredCount);
        }
        catch (Exception ex)
        {
            return BadRequest("Não foi possível restaurar os desafios. Erro: " + ex.Message);
        }
    }
}
