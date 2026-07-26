using backend.models;

namespace backend.Repositories.Interfaces
{
    public interface ISenhaTokenRepository
    {
        Task SalvarAsync(SenhaToken senhaToken);
        Task<SenhaToken?> ObterPorTokenEUsuarioAsync(string token, int usuarioId);
        Task DeletarAsync(SenhaToken senhaToken);
    }
}