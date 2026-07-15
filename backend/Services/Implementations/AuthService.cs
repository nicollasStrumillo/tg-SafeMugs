using backend.DTOs.Auth;
using backend.Exceptions;
using backend.Helpers;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IDesafioService _desafioService;

    public AuthService(IUsuarioRepository usuarioRepository, IDesafioService desafioService)
    {
        _usuarioRepository = usuarioRepository;
        _desafioService = desafioService;
    }

    public async Task CadastrarUsuarioAsync(CadastroRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NomeCompleto) || string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Senha) || string.IsNullOrWhiteSpace(request.ConfirmarSenha))
            throw new ValidationException("Todos os campos são obrigatórios.");

        if (request.Senha != request.ConfirmarSenha)
            throw new ValidationException("As senhas não coincidem.");

        if (request.Senha.Length < 8)
            throw new ValidationException("A senha deve ter pelo menos 8 caracteres.");

        var usuarioExistente = await _usuarioRepository.BuscaPorEmailAsync(request.Email);
        if (usuarioExistente != null)
            throw new BusinessException("Já existe um usuário cadastrado com este e-mail.");

        //Gerar o hash da senha antes de salvar no banco de dados
        string hashSenha = HashHelper.GerarMD5(request.Senha); 
        request.HashSenha = hashSenha;

        if (request.Perfil?.ToLower() == "administrador")
        {
            await CadastrarAdministradorAsync(request, resolverDesafio: true);
        }
        else
        {
            await _usuarioRepository.CadastrarUsuarioAsync(request); 
        }      
    }

    private async Task CadastrarAdministradorAsync(CadastroRequest request, bool resolverDesafio = false)
    {
        if (request.Perfil?.ToLower() != "administrador") return; 

        if (resolverDesafio)
            await _desafioService.SolveIfAsync("Manipular cadastro", () => true);
               
        await _usuarioRepository.CadastrarUsuarioAsync(request);        
    }

    public async Task<LoginResponse?> RealizarLoginAsync(LoginRequest request)
    {
        //Gerar o hash da senha antes de procurar no banco de dados
        request.HashSenha = HashHelper.GerarMD5(request.Senha);
        
        var usuario = await _usuarioRepository.RealizarLoginAsync(request);

        await _desafioService.SolveIfAsync("Login como Admin", () => usuario?.Perfil == "Administrador");

        return usuario;
    }
}
