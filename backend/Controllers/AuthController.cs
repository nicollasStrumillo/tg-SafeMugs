using backend.DTOs.Auth;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> Cadastro([FromBody] CadastroRequest request)
    {
        await _authService.CadastrarUsuarioAsync(request);    
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.RealizarLoginAsync(request);
        if (response == null)
        {
            return Unauthorized(new { detail = "Credenciais inválidas." });
        }
        return Ok(response);
    }
}
