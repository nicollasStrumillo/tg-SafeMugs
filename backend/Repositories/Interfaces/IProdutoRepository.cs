using backend.models;

namespace backend.repositories.Interfaces;

public interface IProdutoRepository
{
    Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default);
}
