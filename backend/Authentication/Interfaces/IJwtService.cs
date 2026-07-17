using backend.DTOs.Auth;

namespace backend.Authentication.Interfaces;

public interface IJwtService
{
    AuthTokenResponse GenerateToken(LoginResponse userData);
}
