using System.IdentityModel.Tokens.Jwt;
using System.Text;
using backend.Authentication;
using backend.DTOs.Desafio;
using backend.models.Enums;
using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers;

[ApiController]
[Route("api/desafios")]
public class DesafioController : ControllerBase
{
    private static readonly byte[] PixelXssCallback = Convert.FromBase64String("R0lGODlhAQABAIAAAP///////yH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==");

    private const string CookieTokenName = "safemugs.token";
    private readonly IDesafioService _desafioService;
    private readonly IDesafiosBackupService _desafiosBackupService;
    private readonly ISugestaoDesafioService _sugestaoDesafioService;
    private readonly JwtSettings _jwt;

    public DesafioController(IDesafioService desafioService, IDesafiosBackupService desafiosBackupService, ISugestaoDesafioService sugestaoDesafioService, IOptions<JwtSettings> jwt)
    {
        _desafioService = desafioService;
        _desafiosBackupService = desafiosBackupService;
        _sugestaoDesafioService = sugestaoDesafioService;
        _jwt = jwt.Value;
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
    public async Task<ActionResult<int>> RestoreDesafiosAsync([FromBody] RestoreRequest request)
    {
        var restoredCount = await _desafiosBackupService.RestoreAsync(request.BackupString);
        return Ok(restoredCount);
    }

    [HttpGet("backupQuizzes")]
    public ActionResult<string?> BackupQuizzesGenerateAsync()
    {
        var backupCode = _desafiosBackupService.BackupQuizzesGenerateAsync();

        return Ok(backupCode);
    }

    [HttpPost("restoreQuizzes")]
    public ActionResult<int> RestoreQuizzesAsync([FromBody] RestoreRequest request)
    {
        var restoredCount = _desafiosBackupService.RestoreQuizzesAsync(request.BackupString);
        return Ok(restoredCount);
    }

    [HttpGet("sugestoes")]
    public async Task<ActionResult<IEnumerable<DesafioResponse>>> ObterSugestoes([FromQuery] int quantidade = 3)
    {
        var sugestoes = await _sugestaoDesafioService.GerarSugestoesAsync(quantidade);
        return Ok(sugestoes);
    }

    #region Desafio DOM clobbering
    [HttpGet("xss-callback")]
    [AllowAnonymous]
    public async Task<IActionResult> ColetarXss([FromQuery] string? t)
    {
        try
        {
            var token = ExtrairToken(t);
            if (token is not null && JwtValido(token))
                await _desafioService.SolveIfAsync(DesafiosEnum.DomClobbering, () => true);
        }
        catch {}

        return File(PixelXssCallback, "image/gif");
    }

    private static string? ExtrairToken(string? cookies)
    {
        if (string.IsNullOrWhiteSpace(cookies))
            return null;

        foreach (var parte in cookies.Split(';'))
        {
            var item = parte.Trim();
            if (item.StartsWith(CookieTokenName + "=", StringComparison.Ordinal))
            {
                var valor = item[(CookieTokenName.Length + 1)..].Trim();
                return string.IsNullOrEmpty(valor) ? null : valor;
            }
        }

        return null;
    }

    private bool JwtValido(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwt.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey)),
            }, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion
}
