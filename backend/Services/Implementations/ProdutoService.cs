using backend.Authentication.Interfaces;
using backend.DTOs.Produto;
using backend.DTOs.Usuario;
using backend.Exceptions;
using backend.models;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUsuarioRepository _usuarioReprository;

    private readonly IAuthenticatedUserService _user;
    private readonly IDesafioService _desafioService;

    public ProdutoService(IProdutoRepository produtoRepository, IAuthenticatedUserService user, IDesafioService desafioService, IUsuarioRepository usuarioRepository)
    {
        _produtoRepository = produtoRepository;
        _usuarioReprository = usuarioRepository;
        _user = user;
        _desafioService = desafioService;
    }

    private ProdutoDTO MapProdutoToDTO(Produto produto)
    {
        return new ProdutoDTO
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            CategoriaProduto = new CategoriaProdutoDTO
            {
                Id = produto.CategoriaProduto.Id,
                Nome = produto.CategoriaProduto.Nome,
                Descricao = produto.CategoriaProduto.Descricao
            },
            UrlImagemProduto = produto.UrlImagemProduto
        };
    }

    private ProdutoCompletoDTO MapProdutoCompletoToDTO(Produto produto)
    {
        return new ProdutoCompletoDTO
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            CategoriaProduto = new CategoriaProdutoDTO
            {
                Id = produto.CategoriaProduto.Id,
                Nome = produto.CategoriaProduto.Nome,
                Descricao = produto.CategoriaProduto.Descricao
            },
            UrlImagemProduto = produto.UrlImagemProduto,
            Avaliacoes = produto.Avaliacoes.Select(a => new AvaliacaoDTO
            {
                Id = a.Id,
                Nota = a.Nota,
                Comentario = a.Comentario,
                Usuario = new UsuarioDetalhesDTO{
                    Id = a.Usuario.Id,
                    NomeCompleto = a.Usuario.NomeCompleto,
                    Telefone = a.Usuario.Telefone,
                    Email = a.Usuario.Email,
                    Ativo = a.Usuario.Ativo,
                    UrlImagemPerfil = a.Usuario.UrlImagemPerfil,
                    Perfil = a.Usuario.Perfil.Nome
                }
            }).ToList(),
            ComentariosProduto = produto.ComentariosProduto.Select(c => new ComentarioProdutoDTO
            {
                Id = c.Id,
                Comentario = c.Comentario,
                Usuario = c.Usuario == null ? null : new UsuarioDetalhesDTO{
                    Id = c.Usuario.Id ,
                    NomeCompleto = c.Usuario.NomeCompleto,
                    Email = c.Usuario.Email,
                    Ativo = c.Usuario.Ativo,
                    UrlImagemPerfil = c.Usuario.UrlImagemPerfil
                }
            }).ToList()
        };
    }

    public async Task<IReadOnlyList<ProdutoCompletoDTO>> ObterTodosAsync()
    {
        var produtos = await _produtoRepository.ObterTodosAsync();
        return [.. produtos.Select(MapProdutoCompletoToDTO)];
    }

    public async Task<ProdutoCompletoDTO?> ObterProdutoCompletoPorIdAsync(int produtoId)
    {
        var produto = await _produtoRepository.ObterProdutoCompletoPorIdAsync(produtoId);
        return produto == null ? null : MapProdutoCompletoToDTO(produto);
    }

    public async Task<ProdutoCompletoDTO?> ObterProdutoCompletoPorNomeAsync(string nome)
    {
        var produto = await _produtoRepository.ObterProdutoPorNomeAsync(nome);
        return produto == null ? null : MapProdutoCompletoToDTO(produto);
    }

    public async Task<List<ComentarioProdutoDTO>> ObterComentariosPorProdutoIdAsync(int produtoId)
    {
        var comentarios = await _produtoRepository.ObterComentariosPorProdutoIdAsync(produtoId);
        return comentarios.Select(c => new ComentarioProdutoDTO
        {
            Id = c.Id,
            Comentario = c.Comentario,
            Usuario = c.Usuario == null ? null : new UsuarioDetalhesDTO
            {
                Id = c.Usuario.Id,
                NomeCompleto = c.Usuario.NomeCompleto,
                Email = c.Usuario.Email,
                Ativo = c.Usuario.Ativo
            }
        }).ToList();
    }

    public async Task FazerComentarioAsync(int produtoId, string? nomeCompleto, string comentario)
    {
        Usuario? usuario = null;

        if (!string.IsNullOrEmpty(nomeCompleto))
        {
            usuario = await _usuarioReprository.BuscarPorNomeAsync(nomeCompleto);
            if (usuario == null)
                throw new NotFoundException($"Usuário {nomeCompleto} não encontrado");
        }
            
        await _produtoRepository.FazerComentarioAsync(produtoId, usuario?.Id, comentario);

        await _desafioService.SolveIfAsync(DesafiosEnum.CriarComentarioOutroUsuario, () => (usuario == null ? 0 : usuario.Id) != _user.UsuarioId);
    }

    public async Task AtualizarComentarioAsync(int comentarioId, string comentario)
    {
        var comentarioEditado = await _produtoRepository.ProcurarComentarioPorIdAsync(comentarioId);
        if (comentarioEditado == null)
            throw new NotFoundException($"Comentário com ID {comentarioId} não encontrado.");

        await _desafioService.SolveIfAsync(DesafiosEnum.AlterarComentarioOutroUsuario, () => comentarioEditado.UsuarioId != _user.UsuarioId);
        
        await _produtoRepository.AtualizarComentarioAsync(comentarioEditado, comentario);
    }
}
