using backend.models;

namespace backend.services.Interfaces;

public interface IProdutoService
{
    Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default);
}
