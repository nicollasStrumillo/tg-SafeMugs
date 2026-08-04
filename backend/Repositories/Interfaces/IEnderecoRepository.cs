using backend.models;

namespace backend.Repositories.Interfaces;

public interface IEnderecoRepository
{
    Task<Endereco> CadastrarEnderecoAsync(Endereco endereco);
    Task DeletarEnderecoAsync(int enderecoId);
}
