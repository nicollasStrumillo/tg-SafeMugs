using backend.DTOs.Desafio;

namespace backend.Services.Interfaces;

public interface IDesafioService
{
    Task<IEnumerable<DesafioResponse>> ObterTodosAsync();
    IEnumerable<string> ObterCategorias();
    Task SolveIfAsync(string nomeDesafio, Func<bool> criteria);
    Task<int> SolveListDesafiosAsync(int[] ids);
}
