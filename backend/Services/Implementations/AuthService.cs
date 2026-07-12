using backend.DTOs.Auth;
using backend.Exceptions;
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
        if (string.IsNullOrWhiteSpace(request.NomeCompleto) || string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Senha) || string.IsNullOrWhiteSpace(request.ConfirmarSenha))
            throw new ValidationException("Todos os campos são obrigatórios.");

        if (request.Senha != request.ConfirmarSenha)
            throw new ValidationException("As senhas não coincidem.");
        
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
