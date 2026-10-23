using backend.models;

namespace backend.Repositories.Interfaces;

public interface IAvaliacaoRepository
{
    Task EscreverAvaliacaoAsync(string comentario, float nota, int usuarioId, int produtoId);
    Task<List<Avaliacao>> ObterTodasAsync();
    Task<Avaliacao?> ObterPorIdAsync(int avaliacaoId);
    Task ExcluirAsync(Avaliacao avaliacao);
}
