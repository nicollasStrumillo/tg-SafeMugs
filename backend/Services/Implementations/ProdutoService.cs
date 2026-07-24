using backend.Authentication.Interfaces;
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

    public async Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _produtoRepository.ObterTodosAsync(cancellationToken);
    }

    public async Task<List<ComentarioProduto>> ObterComentariosPorProdutoIdAsync(int produtoId)
    {
        return await _produtoRepository.ObterComentariosPorProdutoIdAsync(produtoId);
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
