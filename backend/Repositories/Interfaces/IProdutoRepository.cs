using backend.models;

namespace backend.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default);
}
