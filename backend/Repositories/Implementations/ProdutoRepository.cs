using backend.Data;
using backend.models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementations;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDBContext _dbContext;

    public ProdutoRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Produto?> ObterProdutoPorIdAsync(int produtoId)
    {
        return await _dbContext.Produtos.FindAsync(new object[] { produtoId });
    }

    public async Task<IReadOnlyList<Produto>> ObterTodosAsync()
    {
        return await _dbContext.Produtos.AsNoTracking()
        .Include(p => p.Avaliacoes)
            .ThenInclude(a => a.Usuario)
                .ThenInclude(u => u.Perfil)
        .Include(p => p.ComentariosProduto)
            .ThenInclude(c => c.Usuario)
        .Include(p => p.CategoriaProduto)
        .ToListAsync();
    }

    public async Task<Produto?> ObterProdutoPorNomeAsync(string nome)
    {
        return await _dbContext.Produtos
            .Include(p => p.Avaliacoes)
                .ThenInclude(a => a.Usuario)
                    .ThenInclude(u => u.Perfil)
            .Include(p => p.ComentariosProduto)
                .ThenInclude(c => c.Usuario)
            .Include(p => p.CategoriaProduto)
            .FirstOrDefaultAsync(p => p.Nome == nome);
    }

    public async Task<Produto?> ObterProdutoCompletoPorIdAsync(int produtoId)
    {
        return await _dbContext.Produtos
            .Include(p => p.Avaliacoes)
                .ThenInclude(a => a.Usuario)
                    .ThenInclude(u => u.Perfil)
            .Include(p => p.ComentariosProduto)
                .ThenInclude(c => c.Usuario)
            .Include(p => p.CategoriaProduto)
            .FirstOrDefaultAsync(p => p.Id == produtoId);
    }

    public async Task<List<ComentarioProduto>> ObterComentariosPorProdutoIdAsync(int produtoId)
    {
        return await _dbContext.ComentariosProduto
            .Where(c => c.ProdutoId == produtoId)
            .Include(c => c.Usuario)
            .ToListAsync();
    }

    public async Task FazerComentarioAsync(int produtoId, int? usuarioId, string comentario)
    {
        var produto = await _dbContext.Produtos.FindAsync(new object[] { produtoId });
        if (produto == null)
            throw new Exception("Produto não encontrado.");

        var novoComentario = new ComentarioProduto
        {
            ProdutoId = produtoId,
            UsuarioId = usuarioId,
            Comentario = comentario
        };

        _dbContext.ComentariosProduto.Add(novoComentario);
        await _dbContext.SaveChangesAsync();       
    }
    public async Task AtualizarComentarioAsync(ComentarioProduto comentario, string novaDescricao)
    {
        comentario.Comentario = novaDescricao;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ComentarioProduto?> ProcurarComentarioPorIdAsync(int comentarioId)
    {
        var comentario = await _dbContext.ComentariosProduto.FindAsync([comentarioId]);
        return comentario;
    }
}
