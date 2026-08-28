using backend.DTOs.Usuario;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.SsrfConfiguration;
using backend.models.Enums;
using Microsoft.Extensions.Options;

namespace backend.Controllers;

[ApiController]
[Route("api/usuario")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    private readonly IDesafioService _desafioService;
    private readonly SsrfSettings _ssrf;
    private readonly IWebHostEnvironment _env;



    public UsuarioController(IUsuarioService authService, IDesafioService desafioService, IOptions<SsrfSettings> ssrf, IWebHostEnvironment env)
    {
        _usuarioService = authService;
        _desafioService = desafioService;
        _ssrf = ssrf.Value;
        _env = env;
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> Cadastro([FromBody] CadastroRequest request)
    {
        await _usuarioService.CadastrarUsuarioAsync(request);    
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthTokenResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _usuarioService.RealizarLoginAsync(request);
        if (response == null)
        {
            return Unauthorized(new { detail = "Credenciais inválidas." });
        }
        return Ok(response);
    }

    [HttpGet("detalhes/{usuarioId}")]
    public async Task<ActionResult<UsuarioDetalhesDTO>> Detalhes(int usuarioId)
    {
        var response = await _usuarioService.ObterUsuarioDetalhesAsync(usuarioId);
        
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("editar")]
    public async Task<ActionResult<AuthTokenResponse>> Editar([FromBody] EditarUsuarioRequest request)
    {
        var response = await _usuarioService.EditarUsuarioAsync(request);
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("foto-perfil/upload")]
    public async Task<ActionResult<AuthTokenResponse>> UploadFotoPerfil(IFormFile foto)
    {
        var response = await _usuarioService.UploadFotoPerfilAsync(foto);
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("foto-perfil/url")]
    public async Task<ActionResult<AuthTokenResponse>> UploadFotoPerfilUrl([FromBody] UploadFotoPerfilUrlRequest request)
    {
        var response = await _usuarioService.UploadFotoPerfilUrlAsync(request);
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("mudar-senha")]
    public async Task<IActionResult> MudarSenha([FromBody] MudarSenhaRequest request)
    {
        await _usuarioService.MudarSenhaAsync(request);
        return Ok();
    }

    [Authorize]
    [HttpPatch("desativar/{usuarioId}")]
    public async Task<IActionResult> Desativar(int usuarioId)
    {
        await _usuarioService.DesativarUsuarioAsync(usuarioId);
        return Ok();
    }

    // Endpoint para resolver o desafio de SSRF
    [HttpGet("requisicao-interna")]
    public async Task<IActionResult> RequisicaoDesafioSsrf()
    {
        if (Request.Headers.TryGetValue(_ssrf.SecretHeaderName, out var valor) 
        && string.Equals(valor.ToString(), _ssrf.SecretHeaderValue,StringComparison.OrdinalIgnoreCase ))
        {
            await _desafioService.SolveIfAsync(DesafiosEnum.SsrfUploadFotoPerfil, () => true);
            return PhysicalFile(Path.Combine(_env.WebRootPath, "imagens", "produto", "mug_brava.jpg"), "image/jpeg");
        }

        return Unauthorized(new {detail = "Acesso não autorizado."});
    }
}
