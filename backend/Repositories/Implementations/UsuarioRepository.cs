using backend.Data;
using backend.DTOs.Usuario;
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

    public async Task<Usuario?> BuscarPorIdAsync(int usuarioId)
    {
        return await _context.Usuarios
            .Include(u => u.Perfil)
            .Include(u => u.Endereco)
            .FirstOrDefaultAsync(u => u.Id == usuarioId);
    }

    public async Task<Usuario?> BuscaPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> BuscarPorNomeAsync(string nomeCompleto)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NomeCompleto == nomeCompleto);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
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
            PerfilId = 1, // Definindo o PerfilId como 1 (Cliente)
            UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg" // Definindo a URL da imagem de perfil padrão
        };

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();
    }

    public async Task CadastrarAdministradorAsync(CadastroRequest request)
    {
        var novoAdministrador = new Usuario
        {
            NomeCompleto = request.NomeCompleto,
            Email = request.Email,
            HashSenha = request.HashSenha!,
            Telefone = null, 
            Ativo = true,
            DtCadastro = DateTime.UtcNow,
            DtAtualizacao = DateTime.UtcNow,
            PerfilId = 2, // Definindo o PerfilId como 2 (Administrador) 
            UrlImagemPerfil = "/imagens/perfil/generic_admin_profile.jpg" // Definindo a URL da imagem de perfil admin padrão
        };

        _context.Usuarios.Add(novoAdministrador);
        await _context.SaveChangesAsync();
    }

    public async Task<LoginResponse?> RealizarLoginAsync(LoginRequest request)
    {
        var sql = $@"SELECT u.Id AS UsuarioId, u.NomeCompleto, u.Email, u.UrlImagemPerfil, p.Nome AS Perfil FROM usuarios u INNER JOIN perfis p ON u.PerfilId = p.Id WHERE u.Email = '{request.Email}' AND u.HashSenha = '{request.HashSenha}' AND u.Ativo = 1 limit 1;";

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
