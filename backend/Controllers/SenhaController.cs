using backend.DTOs.Auth;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/senha")]
    public class SenhaController:Controller
    {
    private readonly ISenhaService senhaService;

    public SenhaController(ISenhaService senhaService)
    {
        this.senhaService = senhaService;
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetSenhaRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await senhaService.TrocarSenhaAsync(dto);
        return NoContent();
    }
}
}