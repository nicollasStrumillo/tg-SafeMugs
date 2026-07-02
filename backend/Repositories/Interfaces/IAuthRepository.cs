using backend.DTOs.Auth;

namespace backend.Repositories.Interfaces;

public interface IAuthRepository
{
    Task CadastrarUsuarioAsync(CadastroRequest request);
    Task<LoginResponse?> RealizarLoginAsync(LoginRequest request);
}
