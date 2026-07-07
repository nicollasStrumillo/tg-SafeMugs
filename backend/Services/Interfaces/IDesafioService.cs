using backend.DTOs.Desafio;

namespace backend.Services.Interfaces;

public interface IDesafioService
{
    Task<IEnumerable<DesafioResponse>> ObterTodosAsync();
    IEnumerable<string> ObterCategorias();
}
