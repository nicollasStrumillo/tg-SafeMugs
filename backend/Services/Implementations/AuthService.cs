using backend.DTOs.Auth;
using backend.Helpers;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task CadastrarUsuarioAsync(CadastroRequest request)
    {
        if (request.Senha != request.ConfirmarSenha)
            throw new ArgumentException("As senhas não coincidem.");
        
        if (string.IsNullOrWhiteSpace(request.NomeCompleto) || string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Senha) || string.IsNullOrWhiteSpace(request.ConfirmarSenha))
            throw new ArgumentException("Todos os campos são obrigatórios.");
        
        //Gerar o hash da senha antes de salvar no banco de dados
        string hashSenha = HashHelper.GerarMD5(request.Senha); 
        request.HashSenha = hashSenha;

        await _authRepository.CadastrarUsuarioAsync(request);        
    }

    public async Task<LoginResponse?> RealizarLoginAsync(LoginRequest request)
    {
        //Gerar o hash da senha antes de procurar no banco de dados
        request.HashSenha = HashHelper.GerarMD5(request.Senha);
        
        return await _authRepository.RealizarLoginAsync(request);
    }
}
