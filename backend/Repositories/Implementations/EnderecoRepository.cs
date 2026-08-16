using backend.Data;
using backend.models;
using backend.Repositories.Interfaces;

namespace backend.Repositories.Implementations;

public class EnderecoRepository : IEnderecoRepository
{
    private readonly ApplicationDBContext _context;

    public EnderecoRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Endereco> CadastrarEnderecoAsync(Endereco endereco)
    {
        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();
        return endereco;
    }

    public async Task DeletarEnderecoAsync(int enderecoId)
    {
        var endereco = await _context.Enderecos.FindAsync(enderecoId);
        if (endereco != null)
        {
            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Endereco> CriarDuplicataAsync(Endereco endereco)
    {
        var duplicata = new Endereco
        {
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero,
            Complemento = endereco.Complemento,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado,
            Cep = endereco.Cep,
            DtCadastro = DateTime.UtcNow,
            DtAtualizacao = DateTime.UtcNow,
        };

        _context.Enderecos.Add(duplicata);
        await _context.SaveChangesAsync();
        return duplicata;
    }
}
