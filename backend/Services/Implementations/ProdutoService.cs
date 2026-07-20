using backend.Authentication.Interfaces;
using backend.Exceptions;
using backend.models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IAuthenticatedUserService _user;
    private readonly IDesafioService _desafioService;

    public ProdutoService(IProdutoRepository produtoRepository, IAuthenticatedUserService user, IDesafioService desafioService)
    {
        _produtoRepository = produtoRepository;
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

    public async Task FazerComentarioAsync(int produtoId, int? usuarioId, string comentario)
    {
        await _produtoRepository.FazerComentarioAsync(produtoId, usuarioId, comentario);
    }

    public async Task AtualizarComentarioAsync(int comentarioId, string comentario)
    {
        var comentarioEditado = await _produtoRepository.ProcurarComentarioPorIdAsync(comentarioId);
        if (comentarioEditado == null)
            throw new NotFoundException($"Comentário com ID {comentarioId} não encontrado.");

        await _desafioService.SolveIfAsync("Altere o comentário de outro usuário", () => comentarioEditado.UsuarioId != _user.UsuarioId);
        
        await _produtoRepository.AtualizarComentarioAsync(comentarioEditado, comentario);
    }
}
