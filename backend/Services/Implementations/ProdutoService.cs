using backend.Authentication.Interfaces;
using backend.models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IAuthenticatedUserService _user;

    public ProdutoService(IProdutoRepository produtoRepository, IAuthenticatedUserService user)
    {
        _produtoRepository = produtoRepository;
        _user = user;
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
        Console.WriteLine($"Nome token: {_user.NomeCompleto}, Email token: {_user.Email}, Perfil token: {_user.Perfil}, IsAuthenticated: {_user.IsAuthenticated}");

        await _produtoRepository.FazerComentarioAsync(produtoId, usuarioId, comentario);
    }
}
