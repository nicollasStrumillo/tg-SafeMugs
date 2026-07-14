using backend.Data;
using backend.DTOs.Auth;
using backend.models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementations;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDBContext _context;

    public UsuarioRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> BuscaPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task CadastrarUsuarioAsync(CadastroRequest request)
    {
        var novoUsuario = new Usuario
        {
            NomeCompleto = request.NomeCompleto,
            Email = request.Email,
            HashSenha = request.HashSenha!,
            Telefone = null, 
            Ativo = true,
            DtCadastro = DateTime.UtcNow,
            DtAtualizacao = DateTime.UtcNow,
            PerfilId = 1 // Definindo o PerfilId como 1 (Cliente) 
        };

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();
    }

    public async Task<LoginResponse?> RealizarLoginAsync(LoginRequest request)
    {
        var sql = $@"SELECT u.Id AS UsuarioId, u.NomeCompleto, u.Email, p.Nome AS Perfil FROM usuarios u INNER JOIN perfis p ON u.PerfilId = p.Id WHERE u.Email = '{request.Email}' AND u.HashSenha = '{request.HashSenha}' AND u.Ativo = 1 limit 1;";

        try
        {
            var resposta = await _context.Database
                .SqlQueryRaw<LoginResponse>(sql)
                .ToListAsync();
            return resposta.FirstOrDefault();
        }
        catch (MySqlConnector.MySqlException ex)
        {
            throw new InvalidOperationException(
                $"MYSQL_ERROR: {ex.Message}, on query: {sql}", ex);
        }
    }
}
