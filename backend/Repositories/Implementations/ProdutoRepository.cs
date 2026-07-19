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

    public async Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Produtos.AsNoTracking()
        .Include(p => p.Avaliacoes)
            .ThenInclude(a => a.Usuario)
        .Include(p => p.CategoriaProduto)
        .Include(p => p.ImagensProduto)
        .Select(p => new Produto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            Estoque = p.Estoque,
            Ativo = p.Ativo,
            CategoriaProdutoId = p.CategoriaProdutoId,
            CategoriaProduto = new CategoriaProduto
            {
                Id = p.CategoriaProduto.Id,
                Nome = p.CategoriaProduto.Nome
            },
            Avaliacoes = p.Avaliacoes.Select(a => new Avaliacao
            {
                Id = a.Id,
                Nota = a.Nota,
                Comentario = a.Comentario,
                UsuarioId = a.UsuarioId,
                Usuario = new Usuario
                {
                    Id = a.Usuario.Id,
                    NomeCompleto = a.Usuario.NomeCompleto
                }
            }).ToList(),
            ImagensProduto = p.ImagensProduto.Select(i => new ImagemProduto
            {
                Id = i.Id,
                UrlImagem = i.UrlImagem,
                Legenda = i.Legenda
            }).ToList()
        })
        .ToListAsync(cancellationToken);
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
        var comentario = await _dbContext.ComentariosProduto.FindAsync(new object[] { comentarioId });
        return comentario;
    }
}
