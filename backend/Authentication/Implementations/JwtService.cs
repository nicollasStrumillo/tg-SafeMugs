using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Authentication.Interfaces;
using backend.DTOs.Usuario;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace backend.Authentication.Implementations;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

    public JwtService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public AuthTokenResponse GenerateToken(LoginResponse userData)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_settings.SecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userData.UsuarioId.ToString()),
            new(ClaimTypes.Email, userData.Email),
            new(ClaimTypes.Name, userData.NomeCompleto),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, userData.Perfil),
            new("perfil", userData.Perfil),
            new("url_imagem_perfil", userData.UrlImagemPerfil)
        };

        var expiration = DateTime.UtcNow.AddMinutes(_settings.ExpirationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new AuthTokenResponse
        {
            Token = tokenString,
            ExpiresAt = expiration,
            UsuarioId = userData.UsuarioId,
            NomeCompleto = userData.NomeCompleto,
            Email = userData.Email,
            UrlImagemPerfil = userData.UrlImagemPerfil,
            Perfil = userData.Perfil,
        };
    }
}
