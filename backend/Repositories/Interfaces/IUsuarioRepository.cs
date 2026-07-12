using backend.DTOs.Auth;
using backend.models;

namespace backend.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> BuscaPorEmailAsync(string email);
    Task AtualizarAsync(Usuario usuario);
    Task CadastrarUsuarioAsync(CadastroRequest request);
    Task<LoginResponse?> RealizarLoginAsync(LoginRequest request);
}
