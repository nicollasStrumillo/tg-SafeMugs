using backend.models.Enums;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/redirect")]
public class RedirectController : ControllerBase
{
    private readonly IDesafioService _desafioService;
    public RedirectController(IDesafioService desafioService)
    {
        _desafioService = desafioService;
    }

    private static readonly string[] AllowedHosts =
    {
        "https://github.com"
    };

    [HttpGet]
    public async Task<IActionResult> Redirecionar([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("URL não informada.");

        bool isAllowed = AllowedHosts.Any(host => url.Contains(host));

        if (!isAllowed)
            return BadRequest("URL não permitida.");

        bool resolvido = Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && !string.Equals(uri.Host, "github.com", StringComparison.OrdinalIgnoreCase);
        await _desafioService.SolveIfAsync(DesafiosEnum.BurlarAllowlistRedirect, () => resolvido);

        return Redirect(url);
    }
}
