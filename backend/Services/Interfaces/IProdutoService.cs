using backend.models;

namespace backend.Services.Interfaces;

public interface IProdutoService
{
    Task<IReadOnlyList<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default);
}
