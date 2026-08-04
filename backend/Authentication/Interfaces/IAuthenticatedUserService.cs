namespace backend.Authentication.Interfaces;

public interface IAuthenticatedUserService
{
    int UsuarioId { get; }
    string NomeCompleto { get; }
    string Email { get; }
    string Perfil { get; }
    string UrlImagemPerfil { get; }
    bool IsAuthenticated { get; }
}
