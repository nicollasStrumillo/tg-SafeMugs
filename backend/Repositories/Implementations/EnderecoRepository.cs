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
}
