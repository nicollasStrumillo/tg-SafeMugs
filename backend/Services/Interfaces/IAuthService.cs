using backend.DTOs.Auth;

namespace backend.Services.Interfaces;

public interface IAuthService
{
    Task CadastrarUsuarioAsync(CadastroRequest request);
    Task<AuthTokenResponse?> RealizarLoginAsync(LoginRequest request);
    Task EnviarTokenSenhaAsync(string email);
    Task TrocarSenhaAsync(ResetSenhaRequestDto dto);
}
