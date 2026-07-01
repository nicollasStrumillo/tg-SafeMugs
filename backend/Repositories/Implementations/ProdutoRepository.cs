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
        .Include(p => p.ComentariosProduto)
            .ThenInclude(c => c.Usuario)
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
            ComentariosProduto = p.ComentariosProduto.Select(c => new ComentarioProduto
            {
                Id = c.Id,
                Comentario = c.Comentario,
                UsuarioId = c.UsuarioId,
                Usuario = new Usuario
                {
                    Id = c.UsuarioId == null ? 0 : c.Usuario!.Id,
                    NomeCompleto = c.UsuarioId == null ? "Anônimo" : c.Usuario!.NomeCompleto
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
}
