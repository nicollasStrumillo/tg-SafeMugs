using backend.models;

namespace backend.Services.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscaPorEmailAsync(string email);
        Task AtualizarAsync(Usuario usuario);
    }
}