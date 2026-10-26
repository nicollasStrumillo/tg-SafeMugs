using backend.DTOs.Desafio;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces.Util;
using backend.Services.Interfaces.Util.QuizDesafios;

namespace backend.Services.Implementations.Util;

public sealed class DesafioDependencia
{
    public DesafiosEnum Desafio { get; init; }
    public IReadOnlyList<DesafiosEnum> Dependencias { get; init; } = [];
}

public class SugestaoDesafioService : ISugestaoDesafioService
{
    private readonly IDesafioRepository _desafioRepository;
    private readonly IQuizDesafioService _quizDesafioService;

    #region Grafo de dependências dos desafios
    // Cada entrada define um desafio e de 0 a N dependências (outros DesafiosEnum).
    private static readonly IReadOnlyList<DesafioDependencia> Grafo = new List<DesafioDependencia>
    {
        new() { Desafio = DesafiosEnum.LoginAdmin, Dependencias =
            [
                DesafiosEnum.TratamentoErro
            ] },

        new() { Desafio = DesafiosEnum.EncontrarAdministracao, Dependencias = [
                DesafiosEnum.LoginAdmin
            ] },

        new() { Desafio = DesafiosEnum.XxeReposicaoEstoque, Dependencias = [
                DesafiosEnum.LoginAdmin
            ] },

        new() { Desafio = DesafiosEnum.DOS, Dependencias = [] },

        new() { Desafio = DesafiosEnum.DomXss, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.BruteForceLogin, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.CadastroInvalido, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.ManipularCadastro, Dependencias = 
            [
                DesafiosEnum.TratamentoErro
            ] },

        new() { Desafio = DesafiosEnum.AlterarComentarioOutroUsuario, Dependencias = 
            [
                DesafiosEnum.CriarComentarioOutroUsuario
            ] },

        new() { Desafio = DesafiosEnum.StoredXssComentarios, Dependencias = 
            [
                DesafiosEnum.DomXss
            ]},

        new() { Desafio = DesafiosEnum.CriarComentarioOutroUsuario, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.TratamentoErro, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.EnumerarTabelasBanco, Dependencias = 
            [
                DesafiosEnum.LoginAdmin
            ] },

        new() { Desafio = DesafiosEnum.ExtrairHashSenha, Dependencias =
            [
                DesafiosEnum.EnumerarTabelasBanco
            ] },

        new() { Desafio = DesafiosEnum.AdicionarProdutoIndisponivel, Dependencias = 
            [
                DesafiosEnum.VisualizarCarrinhoAlheio
            ] },

        new() { Desafio = DesafiosEnum.VisualizarCarrinhoAlheio, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.ModificarCarrinhoAlheio, Dependencias = 
            [
                DesafiosEnum.VisualizarCarrinhoAlheio
            ] },

        new() { Desafio = DesafiosEnum.CarrinhoQuantidadeInvalida, Dependencias = 
            [
                DesafiosEnum.VisualizarCarrinhoAlheio
            ] },

        new() { Desafio = DesafiosEnum.BurlarAllowlistRedirect, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.SsrfUploadFotoPerfil, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.PathTraversalFotoPerfil, Dependencias = 
            [
                DesafiosEnum.EncontrarScoreBoard
            ] },

        new() { Desafio = DesafiosEnum.ReflectedXss, Dependencias = 
            [
                DesafiosEnum.StoredXssComentarios
            ]},

        new() { Desafio = DesafiosEnum.StoredXssAvaliacoes, Dependencias = 
            [
                DesafiosEnum.ReflectedXss,
                DesafiosEnum.CadastroInvalido
            ]},

        new() { Desafio = DesafiosEnum.CriarAvaliacaoOutroUsuario, Dependencias = 
            [
                DesafiosEnum.CriarAvaliacaoZeroEstrelas,
                DesafiosEnum.CriarComentarioOutroUsuario
            ] },

        new() { Desafio = DesafiosEnum.CriarAvaliacaoZeroEstrelas, Dependencias =
            [
                DesafiosEnum.CadastroInvalido
            ] },

        new() { Desafio = DesafiosEnum.SsrfServicoInternoOculto, Dependencias =
            [
                DesafiosEnum.SsrfUploadFotoPerfil
            ] },

        new() { Desafio = DesafiosEnum.MudarSenhaUsuarioInvalido, Dependencias =
            [
                DesafiosEnum.SsrfServicoInternoOculto
            ] },

        new() { Desafio = DesafiosEnum.DomClobbering, Dependencias =
            [
                DesafiosEnum.StoredXssAvaliacoes
            ] },
    };
    #endregion

    public SugestaoDesafioService(IDesafioRepository desafioRepository, IQuizDesafioService quizDesafioService)
    {
        _desafioRepository = desafioRepository;
        _quizDesafioService = quizDesafioService;
    }

    public async Task<IReadOnlyList<DesafioResponse>> GerarSugestoesAsync(int n = 3)
    {
        if (n <= 0) n = 3;

        var desafios = (await _desafioRepository.ObterTodosAsync()).ToList();

        var resolvidos = new HashSet<DesafiosEnum>();
        foreach (var d in desafios.Where(d => d.Resolvido))
        {
            if (EnumExtensions.TryGetEnumByNomeDisplay<DesafiosEnum>(d.Nome, out var e))
                resolvidos.Add(e);
        }

        string Nome(DesafiosEnum e) => e.GetNomeDisplay();
        var byNome = desafios.ToDictionary(d => d.Nome, d => d);

        var candidatosDesbloqueados = Grafo
            .Where(g => !resolvidos.Contains(g.Desafio))
            .Where(g => g.Dependencias.All(dep => resolvidos.Contains(dep)))
            .Select(g => byNome[Nome(g.Desafio)])
            .OrderBy(d => d.Dificuldade)
            .ThenBy(d => d.Id)
            .ToList();

        if (candidatosDesbloqueados.Count >= n)
            return candidatosDesbloqueados.Take(n).Select(MapToDesafioResponse).ToList();

        var resultado = new List<models.Desafio>(candidatosDesbloqueados);

        if (resultado.Count < n)
        {
            var idsJaSelecionados = new HashSet<int>(resultado.Select(r => r.Id));
            var restantes = desafios
                .Where(d => !d.Resolvido && !idsJaSelecionados.Contains(d.Id))
                .OrderBy(d => d.Dificuldade)
                .ThenBy(d => d.Id);

            foreach (var r in restantes)
            {
                if (resultado.Count >= n) break;
                resultado.Add(r);
            }
        }

        return resultado.Select(MapToDesafioResponse).ToList();
    }

    private DesafioResponse MapToDesafioResponse(models.Desafio desafio)
    {
        QuizDesafios.QuizDesafio? quiz = null;
        if (EnumExtensions.TryGetEnumByNomeDisplay<DesafiosEnum>(desafio.Nome, out var enumDesafio))
            quiz = _quizDesafioService.GetQuizDesafio(enumDesafio);

        return new DesafioResponse
        {
            Id = desafio.Id,
            Nome = desafio.Nome,
            Descricao = desafio.Descricao,
            Categoria = desafio.Categoria.GetNomeDisplay(),
            Dificuldade = desafio.Dificuldade,
            Resolvido = desafio.Resolvido,
            Rota = desafio.Rota,
            PossuiQuiz = quiz != null,
            QuizResolvido = quiz?.Resolvido ?? false,
            DicasDesafio = desafio.DicasDesafio.Select(di => new DicaDesafioDTO
            {
                Id = di.Id,
                NrDica = di.NrDica,
                Texto = di.Texto
            }).ToList()
        };
    }
}
