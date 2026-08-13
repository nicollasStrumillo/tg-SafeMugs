namespace backend.Repositories.Interfaces;

public interface IAvaliacaoRepository
{
    Task EscreverAvaliacaoAsync(string comentario, float nota, int usuarioId, int produtoId);
}
