using backend.models;

namespace backend.Repositories.Interfaces;
public interface IDesafioRepository
{
    Task<IEnumerable<Desafio>> ObterTodosAsync();
    Task<Desafio?> FindByNameAsync(string nomeDesafio);
    Task<List<Desafio>> FindByIdsAsync(int[] ids);
    Task ResolverDesafioAsync(Desafio desafio);
}
