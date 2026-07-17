namespace backend.Authentication.Interfaces;

public interface IAuthenticatedUserService
{
    int UsuarioId { get; }
    string NomeCompleto { get; }
    string Email { get; }
    string Perfil { get; }
    bool IsAuthenticated { get; }
}
