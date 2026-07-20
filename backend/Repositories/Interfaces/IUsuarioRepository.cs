using backend.DTOs.Auth;
using backend.models;

namespace backend.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> BuscaPorEmailAsync(string email);
    Task<Usuario?> BuscarPorNomeAsync(string nomeCompleto);
    Task AtualizarAsync(Usuario usuario);
    Task CadastrarUsuarioAsync(CadastroRequest request);
     Task CadastrarAdministradorAsync(CadastroRequest request);
    Task<LoginResponse?> RealizarLoginAsync(LoginRequest request);
}
