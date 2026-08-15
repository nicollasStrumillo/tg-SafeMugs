using backend.Data;
using backend.models;
using backend.Repositories.Interfaces;

namespace backend.Repositories.Implementations;

public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly ApplicationDBContext _dbContext;

    public AvaliacaoRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task EscreverAvaliacaoAsync(string comentario, float nota, int usuarioId, int produtoId)
    {
        var avaliacao = new Avaliacao
        {
            Nota = nota,
            Comentario = comentario,      
            UsuarioId = usuarioId,
            ProdutoId = produtoId,
            DtCadastro = DateTime.UtcNow,
            DtAtualizacao = DateTime.UtcNow
        };

        _dbContext.Avaliacoes.Add(avaliacao);
        await _dbContext.SaveChangesAsync();
    }
}