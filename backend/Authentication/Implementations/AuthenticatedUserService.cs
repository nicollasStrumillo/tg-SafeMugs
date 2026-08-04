using System.Security.Claims;
using backend.Authentication.Interfaces;

namespace backend.Authentication.Implementations;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UsuarioId
    {
        get
        {
            var sub = _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(sub, out var id) ? id : 0;
        }
    }

    public string NomeCompleto =>
        _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.Name)
        ?? string.Empty;

    public string Email =>
        _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.Email)
        ?? string.Empty;

    public string Perfil =>
        _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.Role)
        ?? string.Empty;

    public string UrlImagemPerfil =>
        _httpContextAccessor.HttpContext?.User?
            .FindFirstValue("url_imagem_perfil")
        ?? string.Empty;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?
            .Identity?.IsAuthenticated ?? false;
}
