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
        var usuario = await _context.Usuarios.Include(u => u.Perfil).FirstOrDefaultAsync(u => u.Email == request.Email && u.HashSenha == request.HashSenha);
        if (usuario == null) return null;

        return new LoginResponse
        {
            UsuarioId = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            Perfil = usuario.Perfil.Nome
        };
    }
}
