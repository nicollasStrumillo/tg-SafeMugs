using backend.Authentication.Interfaces;
using backend.DTOs.Auth;
using backend.Exceptions;
using backend.Helpers;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using System.Net.Mail;

namespace backend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IDesafioService _desafioService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IDesafioService desafioService,
        IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _desafioService = desafioService;
        _jwtService = jwtService;
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

        await _desafioService.SolveIfAsync("Cadastro inválido", () => !IsValidEmail(request.Email));
    }

    private async Task CadastrarAdministradorAsync(CadastroRequest request, bool resolverDesafio = false)
    {
        if (request.Perfil?.ToLower() != "administrador") return; 

        if (resolverDesafio)
            await _desafioService.SolveIfAsync("Manipular cadastro", () => true);
               
        await _usuarioRepository.CadastrarUsuarioAsync(request);        
    }

    public async Task<AuthTokenResponse?> RealizarLoginAsync(LoginRequest request)
    {
        request.HashSenha = HashHelper.GerarMD5(request.Senha);

        var usuario = await _usuarioRepository.RealizarLoginAsync(request);

        if (usuario == null)
            return null;

        await _desafioService.SolveIfAsync("Login como Admin", () => usuario.Perfil == "Administrador" && request.ResolverDesafioSqlInjection);
        await ResolverDesafiosBruteForceAsync(request.Email, request.HashSenha);

        return _jwtService.GenerateToken(usuario);
    }


    private async Task ResolverDesafiosBruteForceAsync(string email, string hashSenha)
    {
        bool resolvido = new[]
        {
            EmailsESenhasUsuarios.AnaLopes,
            EmailsESenhasUsuarios.BrunoCosta,
            EmailsESenhasUsuarios.CarlaMendes,
            EmailsESenhasUsuarios.DiegoSouza,
            EmailsESenhasUsuarios.ElisaMartins,
            EmailsESenhasUsuarios.FelipeRocha,
            EmailsESenhasUsuarios.MarinaAlves,
        }.Any(u => email == u.GetNomeDisplay() && hashSenha == u.GetHashSenha().ToUpper());;

        await _desafioService.SolveIfAsync("Brute force de login", () => resolvido);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
