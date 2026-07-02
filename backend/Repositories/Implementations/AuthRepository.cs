using backend.Data;
using backend.DTOs.Auth;
using backend.models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDBContext _dbContext;

    public AuthRepository(ApplicationDBContext context)
    {
        _dbContext = context;
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

        _dbContext.Usuarios.Add(novoUsuario);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<LoginResponse?> RealizarLoginAsync(LoginRequest request)
    {
        var usuario = await _dbContext.Usuarios.Include(u => u.Perfil).FirstOrDefaultAsync(u => u.Email == request.Email && u.HashSenha == request.HashSenha);
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

