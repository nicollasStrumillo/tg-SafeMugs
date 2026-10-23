using backend.DTOs.Produto;

namespace backend.Services.Interfaces;

public interface IReposicaoEstoqueService
{
    Task<ReposicaoEstoqueResultadoDTO> ImportarAsync(Stream xml);
}
