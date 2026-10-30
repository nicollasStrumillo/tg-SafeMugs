using System.IdentityModel.Tokens.Jwt;
using backend.DTOs.Usuario;

namespace backend.Authentication.Interfaces;

public interface IJwtService
{
    AuthTokenResponse GenerateToken(LoginResponse userData);
    public JwtSecurityToken? ObterTokenBruto(HttpRequest request);
    public bool ValidaAssinatura(JwtSecurityToken jwt);
}
