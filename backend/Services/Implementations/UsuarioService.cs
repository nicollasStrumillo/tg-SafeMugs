using backend.Authentication.Interfaces;
using backend.Data;
using backend.DTOs.Usuario;
using backend.Exceptions;
using backend.Helpers;
using backend.models;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using System.Net.Mail;
using System.Text;
using backend.SsrfConfiguration;
using Microsoft.Extensions.Options;

namespace backend.Services.Implementations;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEnderecoRepository _enderecoRepository;

    private readonly IDesafioService _desafioService;

    private readonly IAuthenticatedUserService _user;
    private readonly IJwtService _jwtService;
    private readonly SsrfSettings _ssrf;

    private readonly ApplicationDBContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IEnderecoRepository enderecoRepository,
        IDesafioService desafioService,
        IAuthenticatedUserService user,
        IJwtService jwtService,
        ApplicationDBContext context,
        IWebHostEnvironment env,
        IHttpClientFactory httpClientFactory,
        IOptions<SsrfSettings> ssrfSettings)
    {
        _usuarioRepository = usuarioRepository;
        _enderecoRepository = enderecoRepository;
        _desafioService = desafioService;
        _user = user;
        _jwtService = jwtService;
        _context = context;
        _env = env;
        _httpClientFactory = httpClientFactory;
        _ssrf = ssrfSettings.Value;
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

        await _desafioService.SolveIfAsync(DesafiosEnum.CadastroInvalido, () => !IsValidEmail(request.Email));
    }

    private async Task CadastrarAdministradorAsync(CadastroRequest request, bool resolverDesafio = false)
    {
        if (request.Perfil?.ToLower() != "administrador") return; 

        if (resolverDesafio)
            await _desafioService.SolveIfAsync(DesafiosEnum.ManipularCadastro, () => true);
               
        await _usuarioRepository.CadastrarAdministradorAsync(request);        
    }

    public async Task<AuthTokenResponse?> RealizarLoginAsync(LoginRequest request)
    {
        request.HashSenha = HashHelper.GerarMD5(request.Senha);

        var usuario = await _usuarioRepository.RealizarLoginAsync(request);

        if (usuario == null || string.Equals(usuario.Email, EmailsESenhasUsuarios.usuarioInvalidoDesafio.GetNomeDisplay()))
            return null;

        await _desafioService.SolveIfAsync(DesafiosEnum.LoginAdmin, () => usuario.Perfil == "Administrador" && request.ResolverDesafioSqlInjection);
        await ResolverDesafiosBruteForceAsync(request.Email, request.HashSenha);

        return _jwtService.GenerateToken(usuario);
    }

    public async Task<UsuarioDetalhesDTO> ObterUsuarioDetalhesAsync(int usuarioId)
    {
        var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId)
            ?? throw new NotFoundException("Usuário não encontrado.");

        var detalhes = new UsuarioDetalhesDTO
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            Telefone = usuario.Telefone,
            Ativo = usuario.Ativo,
            DtCadastro = usuario.DtCadastro,
            DtAtualizacao = usuario.DtAtualizacao,
            UrlImagemPerfil = usuario.UrlImagemPerfil,
            Perfil = usuario.Perfil.Nome,
            Endereco = usuario.Endereco != null ? new DTOs.Endereco.EnderecoDTO
            {
                Logradouro = usuario.Endereco.Logradouro,
                Numero = usuario.Endereco.Numero,
                Complemento = usuario.Endereco.Complemento,
                Bairro = usuario.Endereco.Bairro,
                Cidade = usuario.Endereco.Cidade,
                Estado = usuario.Endereco.Estado,
                Cep = usuario.Endereco.Cep
            } : null
        };

        await _desafioService.SolveIfAsync(
            DesafiosEnum.VisualizarDadosOutroUsuario,
            () => usuario.Id != _user.UsuarioId);

        return detalhes;
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
        }.Any(u => email == u.GetNomeDisplay() && hashSenha == u.GetDescription().ToUpper());;

        await _desafioService.SolveIfAsync(DesafiosEnum.BruteForceLogin, () => resolvido);
    }

    public async Task<AuthTokenResponse?> EditarUsuarioAsync(EditarUsuarioRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NomeCompleto))
            throw new ValidationException("O nome completo é obrigatório.");

        if (!string.IsNullOrWhiteSpace(request.Telefone) && request.Telefone.Length > 11)
            throw new ValidationException("O telefone não pode ter mais de 11 caracteres.");

        var camposObrigatoriosEndereco = new[]
        {
            request.Logradouro,
            request.Bairro,
            request.Cidade,
            request.Estado,
            request.Cep
        };

        bool preencheuAlgumCampoEndereco = request.Numero.HasValue || !string.IsNullOrWhiteSpace(request.Complemento) || camposObrigatoriosEndereco.Any(c => !string.IsNullOrWhiteSpace(c));
   
        bool preencheuTodosObrigatorios = request.Numero.HasValue && camposObrigatoriosEndereco.All(c => !string.IsNullOrWhiteSpace(c));

        if (preencheuAlgumCampoEndereco && !preencheuTodosObrigatorios)
            throw new ValidationException("Preencha todos os campos obrigatórios do endereço ou deixe todos vazios para remover o endereço atual.");

        var usuario = await _usuarioRepository.BuscarPorIdAsync(_user.UsuarioId)
            ?? throw new NotFoundException("Usuário não encontrado.");

        bool jaPossuiEndereco = usuario.Endereco != null;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        usuario.NomeCompleto = request.NomeCompleto;
        usuario.Telefone = request.Telefone;
        usuario.DtAtualizacao = DateTime.UtcNow;

        if (preencheuTodosObrigatorios)
        {
            if (jaPossuiEndereco)
            {
                usuario.Endereco!.Logradouro = request.Logradouro!;
                usuario.Endereco.Numero = request.Numero!.Value;
                usuario.Endereco.Complemento = request.Complemento;
                usuario.Endereco.Bairro = request.Bairro!;
                usuario.Endereco.Cidade = request.Cidade!;
                usuario.Endereco.Estado = request.Estado!;
                usuario.Endereco.Cep = request.Cep!;
                usuario.Endereco.DtAtualizacao = DateTime.UtcNow;

                ValidarEnderecoOrThrow(usuario.Endereco);
            }
            else
            {
                var novoEndereco = new Endereco
                {
                    Logradouro = request.Logradouro!,
                    Numero = request.Numero!.Value,
                    Complemento = request.Complemento,
                    Bairro = request.Bairro!,
                    Cidade = request.Cidade!,
                    Estado = request.Estado!,
                    Cep = request.Cep!,
                    DtCadastro = DateTime.UtcNow,
                    DtAtualizacao = DateTime.UtcNow,
                    Usuario = usuario
                };

                ValidarEnderecoOrThrow(novoEndereco);
                await _enderecoRepository.CadastrarEnderecoAsync(novoEndereco);
                usuario.EnderecoId = novoEndereco.Id;
                usuario.Endereco = novoEndereco;
            }
        }
        else if (jaPossuiEndereco)
        {
            await _enderecoRepository.DeletarEnderecoAsync(usuario.EnderecoId!.Value);
            usuario.EnderecoId = null;
            usuario.Endereco = null;
        }

        await _usuarioRepository.AtualizarAsync(usuario);
        await transaction.CommitAsync();

        return _jwtService.GenerateToken(new LoginResponse
        {
            UsuarioId = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            UrlImagemPerfil = usuario.UrlImagemPerfil,
            Perfil = usuario.Perfil.Nome
        });
    }

    private static void ValidarEnderecoOrThrow(Endereco endereco)
    {
        //Validações de negócio:
        if (endereco.Logradouro.Length > 150)
            throw new ValidationException("O logradouro não pode ter mais de 150 caracteres.");

        if (endereco.Numero <= 0)
            throw new ValidationException("O número do endereço deve ser positivo.");

        if (endereco.Bairro.Length > 100)
            throw new ValidationException("O bairro não pode ter mais de 100 caracteres.");

        if (endereco.Cidade.Length > 100)
            throw new ValidationException("A cidade não pode ter mais de 100 caracteres.");

        if (endereco.Estado.Length > 100)
            throw new ValidationException("O estado não pode ter mais de 100 caracteres.");

        if (endereco.Cep.Length > 20)
            throw new ValidationException("O CEP não pode ter mais de 20 caracteres.");
        if (endereco.Cep.Contains(' ') || endereco.Cep.Any(c => !char.IsDigit(c)))
            throw new ValidationException("O CEP deve conter apenas números.");

        if (endereco.Complemento != null && endereco.Complemento.Length > 200)
            throw new ValidationException("O complemento não pode ter mais de 200 caracteres.");
    }

    public async Task<AuthTokenResponse?> UploadFotoPerfilAsync(IFormFile foto)
    {
        if (foto == null || foto.Length == 0)
            throw new ValidationException("Nenhum arquivo enviado.");

        if (foto.ContentType != "image/jpeg")
            throw new ValidationException("Apenas imagens JPEG são permitidas.");

        var usuario = await _usuarioRepository.BuscarPorIdAsync(_user.UsuarioId)
            ?? throw new NotFoundException("Usuário não encontrado.");

        string pastaUsuario = "Usuario_" + usuario.Id.ToString();
        var pastaDestino = Path.Combine(_env.WebRootPath!, "imagens", "perfil", pastaUsuario);

        Directory.CreateDirectory(pastaDestino);

        // VULNERAVEL A PATH TRAVERSAL
        var caminhoCompleto = Path.Combine(pastaDestino, foto.FileName);

        // RASTREAR PATH TRAVERSAL: Identifica se o upload vai sobreescrever um arquivo 
        bool houveSobrescrita = File.Exists(caminhoCompleto); 

        await using var stream = new FileStream(caminhoCompleto, FileMode.Create);
        await foto.CopyToAsync(stream);

        // Normaliza o caminho (resolve ../) para gravar URL limpa no banco.
        var caminhoNormalizado = Path.GetFullPath(caminhoCompleto);
        var caminhoRelativo = Path.GetRelativePath(_env.WebRootPath!, caminhoNormalizado)
            .Replace('\\', '/');
        var urlImagem = "/" + caminhoRelativo;

        // RASTREAR PATH TRAVERSAL: Identifica se o upload final foi na pasta /produto
        var produtoDir = Path.GetFullPath(Path.Combine(_env.WebRootPath!, "imagens", "produto"));
        bool naPastaProduto = caminhoNormalizado.StartsWith(produtoDir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);

        usuario.UrlImagemPerfil = urlImagem;
        usuario.DtAtualizacao = DateTime.UtcNow;

        await _usuarioRepository.AtualizarAsync(usuario);

        await _desafioService.SolveIfAsync(DesafiosEnum.PathTraversalFotoPerfil, () => houveSobrescrita && naPastaProduto);

        return _jwtService.GenerateToken(new LoginResponse
        {
            UsuarioId = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            UrlImagemPerfil = usuario.UrlImagemPerfil,
            Perfil = usuario.Perfil.Nome
        });
    }

    public async Task<AuthTokenResponse?> UploadFotoPerfilUrlAsync(UploadFotoPerfilUrlRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Url))
            throw new ValidationException("URL não informada.");

        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new ValidationException("URL inválida.");
        
        var usuario = await _usuarioRepository.BuscarPorIdAsync(_user.UsuarioId)
            ?? throw new NotFoundException("Usuário não encontrado.");

        var client = _httpClientFactory.CreateClient("fotoPerfil");
        using var requestTimeout = new CancellationTokenSource(client.Timeout);

        // Verirfica se o desafio de SSRF foi solucionado
        using var req = new HttpRequestMessage(HttpMethod.Get, request.Url);
        if (EhTargetSsrf(uri))
            req.Headers.Add(_ssrf.SecretHeaderName, _ssrf.SecretHeaderValue);

        using var response = await EnviarRequisicaoFotoUrlAsync(client, req, uri, requestTimeout.Token);

        if (!response.IsSuccessStatusCode)
        {
            var trechoResposta = await LerTrechoCorpoRespostaAsync(response.Content, requestTimeout.Token);
            throw new BusinessException(
                $"Falha ao baixar a imagem: {(int)response.StatusCode}. " +
                $"Resposta: {trechoResposta}");
        }

        await _desafioService.SolveIfAsync(
            DesafiosEnum.SsrfServicoInternoOculto,
            () => uri.Port == 9090
                && _ssrf.TargetHost.Contains(uri.Host.ToLowerInvariant())
                && string.Equals(uri.AbsolutePath.TrimEnd('/'), string.Empty, StringComparison.OrdinalIgnoreCase)
        );

        var contentType = response.Content.Headers.ContentType?.MediaType;
        if (string.IsNullOrEmpty(contentType) || !contentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase))
        {
            var trechoResposta = await LerTrechoCorpoRespostaAsync(response.Content, requestTimeout.Token);
            var contentTypeDescricao = contentType ?? "ausente";
            throw new ValidationException(
                $"A URL não retornou uma imagem JPEG (Content-Type: {contentTypeDescricao}). " +
                $"Resposta: {trechoResposta}");
        }

        var bytes = await response.Content.ReadAsByteArrayAsync(requestTimeout.Token);

        string pastaUsuario = "Usuario_" + usuario.Id.ToString();

        var pastaDestino = Path.Combine(_env.WebRootPath!, "imagens", "perfil", pastaUsuario);
        Directory.CreateDirectory(pastaDestino);

        var nomeArquivo = $"{usuario.Id}.jpg";
        var caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

        await File.WriteAllBytesAsync(caminhoCompleto, bytes);

        usuario.UrlImagemPerfil = $"/imagens/perfil/{pastaUsuario}/{nomeArquivo}";
        usuario.DtAtualizacao = DateTime.UtcNow;

        await _usuarioRepository.AtualizarAsync(usuario);

        return _jwtService.GenerateToken(new LoginResponse
        {
            UsuarioId = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            UrlImagemPerfil = usuario.UrlImagemPerfil,
            Perfil = usuario.Perfil.Nome
        });
    }

    private const int MaxTrechoRespostaCaracteres = 200;

    private static async Task<HttpResponseMessage> EnviarRequisicaoFotoUrlAsync(
        HttpClient client,
        HttpRequestMessage request,
        Uri uri,
        CancellationToken cancellationToken)
    {
        try
        {
            return await client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new BusinessException(
                $"Falha de conexão com {uri.Host}:{uri.Port}: {ex.Message}");
        }
        catch (OperationCanceledException)
        {
            throw new BusinessException(
                $"Tempo limite ao conectar com {uri.Host}:{uri.Port}.");
        }
    }

    private static async Task<string> LerTrechoCorpoRespostaAsync(HttpContent content, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(
            await content.ReadAsStreamAsync(cancellationToken),
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: true);
        var buffer = new char[MaxTrechoRespostaCaracteres + 1];
        var totalLido = 0;

        while (totalLido < buffer.Length)
        {
            var lido = await reader.ReadAsync(buffer.AsMemory(totalLido), cancellationToken);
            if (lido == 0)
                break;

            totalLido += lido;
        }

        if (totalLido == 0)
            return "(corpo vazio)";

        var trecho = new string(buffer, 0, Math.Min(totalLido, MaxTrechoRespostaCaracteres)).Trim();
        if (string.IsNullOrWhiteSpace(trecho))
            return "(corpo vazio)";

        return totalLido > MaxTrechoRespostaCaracteres
            ? $"{trecho}…"
            : trecho;
    }

    private bool EhTargetSsrf(Uri uri)
    {
        return string.Equals(uri.Scheme, _ssrf.TargetScheme, StringComparison.OrdinalIgnoreCase)
            && _ssrf.TargetHost.Contains(uri.Host.ToLower())
            && uri.Port == _ssrf.TargetPort
            && string.Equals(uri.AbsolutePath.TrimEnd('/'), _ssrf.TargetPath.TrimEnd('/'), StringComparison.OrdinalIgnoreCase);
    }

    public async Task MudarSenhaAsync(MudarSenhaRequest request)
    {
        if (request.NovaSenha?.Length < 8)
            throw new ValidationException("A nova senha deve ter pelo menos 8 caracteres.");

        var usuario = await _usuarioRepository.BuscarPorIdAsync(_user.UsuarioId)
            ?? throw new NotFoundException("Usuário não encontrado.");

        usuario.HashSenha = HashHelper.GerarMD5(request.NovaSenha!);
        usuario.DtAtualizacao = DateTime.UtcNow;

        await _usuarioRepository.AtualizarAsync(usuario);

        await _desafioService.SolveIfAsync(DesafiosEnum.MudarSenhaUsuarioInvalido, () => usuario.Email == EmailsESenhasUsuarios.usuarioInvalidoDesafio.GetNomeDisplay());
    }

    public async Task DesativarUsuarioAsync(int usuarioId)
    {
        var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId) 
            ?? throw new NotFoundException("Usuário não encontrado.");

        if (_user.UsuarioId != usuarioId)
            throw new UnauthorizedAccessException("Você não tem permissão para desativar este usuário.");

        usuario.Ativo = false;
        usuario.DtAtualizacao = DateTime.UtcNow;

        await _usuarioRepository.AtualizarAsync(usuario);
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
