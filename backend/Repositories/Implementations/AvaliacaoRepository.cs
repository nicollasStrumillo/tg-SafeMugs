using backend.Data;
using backend.models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Avaliacao>> ObterTodasAsync()
    {
        return await _dbContext.Avaliacoes
            .Include(a => a.Usuario)
            .Include(a => a.Produto)
            .OrderByDescending(a => a.DtCadastro)
            .ToListAsync();
    }

    public async Task<Avaliacao?> ObterPorIdAsync(int avaliacaoId)
    {
        return await _dbContext.Avaliacoes
            .Include(a => a.Usuario)
            .Include(a => a.Produto)
            .FirstOrDefaultAsync(a => a.Id == avaliacaoId);
    }

    public async Task ExcluirAsync(Avaliacao avaliacao)
    {
        _dbContext.Avaliacoes.Remove(avaliacao);
        await _dbContext.SaveChangesAsync();
    }
}