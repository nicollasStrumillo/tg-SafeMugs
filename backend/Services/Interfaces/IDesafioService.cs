using backend.DTOs.Desafio;
using backend.models.Enums;

namespace backend.Services.Interfaces;

public interface IDesafioService
{
    Task<IEnumerable<DesafioResponse>> ObterTodosAsync(bool resolverScoreBoard = true);
    IEnumerable<string> ObterCategorias();
    Task<DesafioResponse?> ObterPorNomeAsync(string nomeDesafio);
    Task SolveIfAsync(DesafiosEnum desafio, Func<bool> criteria);
    Task<int> SolveListDesafiosAsync(int[] ids);
}
