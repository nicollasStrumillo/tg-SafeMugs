using backend.DTOs.Usuario;

namespace backend.Authentication.Interfaces;

public interface IJwtService
{
    AuthTokenResponse GenerateToken(LoginResponse userData);
}
