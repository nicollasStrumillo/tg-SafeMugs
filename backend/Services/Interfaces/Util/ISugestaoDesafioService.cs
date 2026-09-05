using backend.DTOs.Desafio;

namespace backend.Services.Interfaces.Util;

public interface ISugestaoDesafioService
{
    Task<IReadOnlyList<DesafioResponse>> GerarSugestoesAsync(int n = 3);
}
