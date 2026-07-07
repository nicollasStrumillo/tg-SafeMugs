using backend.models;

namespace backend.Repositories.Interfaces;
public interface IDesafioRepository
{
    Task<IEnumerable<Desafio>> ObterTodosAsync();
}
