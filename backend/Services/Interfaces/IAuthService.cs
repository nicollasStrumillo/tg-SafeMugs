using backend.DTOs.Auth;

namespace backend.Services.Interfaces;

public interface IAuthService
{
    Task CadastrarUsuarioAsync(CadastroRequest request);
    Task<AuthTokenResponse?> RealizarLoginAsync(LoginRequest request);
}
