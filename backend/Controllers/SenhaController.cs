using backend.DTOs.Auth;
using backend.Services.Implementations;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/senha")]
    public class SenhaController:Controller
    {
    private readonly IAuthService authService;

    public SenhaController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("token")]
    public async Task<IActionResult> EnvioToken([FromBody] string email)
    {
        authService.EnviarTokenSenha(email);
        
        return Ok();
    }
    
    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetSenhaRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await authService.TrocarSenhaAsync(dto);
        return NoContent();
    }

}
}