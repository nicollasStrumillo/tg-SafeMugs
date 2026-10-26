using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using backend.DTOs.Produto;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class ReposicaoEstoqueService : IReposicaoEstoqueService
{
    private const int TamanhoMaximoValorExibido = 10_000;
    private static readonly TimeSpan LimiteTempoDos = TimeSpan.FromSeconds(8);
    private const string ConteudoXxe = "Desafio de XXE resolvido!";

    private readonly IProdutoRepository _produtoRepository;
    private readonly IDesafioService _desafioService;

    public ReposicaoEstoqueService(IProdutoRepository produtoRepository, IDesafioService desafioService)
    {
        _produtoRepository = produtoRepository;
        _desafioService = desafioService;
    }

    public async Task<ReposicaoEstoqueResultadoDTO> ImportarAsync(Stream xml)
    {
        XDocument documento;
        using var token = new CancellationTokenSource();
        Task? task = null;

        try
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                XmlResolver = new XmlUrlResolver(),
                MaxCharactersFromEntities = 0,
            };

            using var reader = XmlReader.Create(xml, settings, new Uri(AppContext.BaseDirectory).AbsoluteUri);
            task = MonitorarDemoraDoXmlAsync(token.Token);
            documento = XDocument.Load(reader, LoadOptions.None);
        }
        catch (Exception ex)
        {
            token.Cancel();
            if (task is not null)
                await task;

            return FalhaGeral($"Falha ao interpretar o XML: {ex.Message}");
        }

        token.Cancel();
        if (task is not null)
            await task;

        if (documento.Root is null || documento.Root.Name.LocalName != "reposicao")
            return FalhaGeral("Formato inesperado: o elemento raiz do arquivo deve ser <reposicao>.");

        var saidaXml = documento.ToString(SaveOptions.DisableFormatting);
        await _desafioService.SolveIfAsync(
            DesafiosEnum.XxeReposicaoEstoque,
            () => saidaXml.Contains(ConteudoXxe, StringComparison.Ordinal));

        var linhas = new List<ReposicaoEstoqueLinhaDTO>();
        var quantidadesPorProduto = new Dictionary<int, long>();

        foreach (var elementoProduto in documento.Root.Elements("produto"))
        {
            linhas.Add(MontarLinha(elementoProduto, quantidadesPorProduto));
        }

        if (linhas.Any(linha => !linha.Sucesso))
            return LoteInvalido(linhas);

        if (quantidadesPorProduto.Count == 0)
        {
            return new ReposicaoEstoqueResultadoDTO
            {
                Sucesso = true,
                TotalLinhas = 0,
                LinhasProcessadas = 0,
                Linhas = []
            };
        }

        var produtos = await _produtoRepository.ObterPorIdsAsync(quantidadesPorProduto.Keys);
        var produtoPorId = produtos.ToDictionary(p => p.Id);

        var haProdutoInexistente = false;
        foreach (var linha in linhas)
        {
            if (linha.ProdutoId is null) continue;

            if (!produtoPorId.TryGetValue(linha.ProdutoId.Value, out var produto))
            {
                linha.Sucesso = false;
                linha.Erro = $"Produto com ID {linha.ProdutoId} não encontrado.";
                haProdutoInexistente = true;
                continue;
            }

            linha.ProdutoNome = produto.Nome;
        }

        if (haProdutoInexistente)
            return LoteInvalido(linhas);

        var estoqueAnteriorPorId = produtos.ToDictionary(p => p.Id, p => p.Estoque);

        var reposicoes = quantidadesPorProduto.ToDictionary(
            par => par.Key,
            par => checked((int)par.Value));

        await _produtoRepository.AplicarReposicaoAsync(reposicoes);

        var jaAplicadoPorId = new Dictionary<int, int>();
        foreach (var linha in linhas)
        {
            if (linha.ProdutoId is null || linha.Quantidade is null) continue;

            var produtoId = linha.ProdutoId.Value;
            jaAplicadoPorId.TryGetValue(produtoId, out var jaAplicado);

            linha.EstoqueAnterior = estoqueAnteriorPorId[produtoId] + jaAplicado;
            linha.EstoqueNovo = linha.EstoqueAnterior + linha.Quantidade.Value;

            jaAplicadoPorId[produtoId] = jaAplicado + linha.Quantidade.Value;
        }

        return new ReposicaoEstoqueResultadoDTO
        {
            Sucesso = true,
            TotalLinhas = linhas.Count,
            LinhasProcessadas = linhas.Count,
            Linhas = linhas
        };
    }

    private async Task MonitorarDemoraDoXmlAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(LimiteTempoDos, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await _desafioService.SolveIfAsync(DesafiosEnum.DOS, () => true);
    }

    private static ReposicaoEstoqueLinhaDTO MontarLinha(XElement elementoProduto, Dictionary<int, long> quantidadesPorProduto)
    {
        var idRecebido = (elementoProduto.Element("id")?.Value ?? string.Empty).Trim();
        var quantidadeRecebida = (elementoProduto.Element("quantidade")?.Value ?? string.Empty).Trim();

        var linha = new ReposicaoEstoqueLinhaDTO();

        if (idRecebido.Length > TamanhoMaximoValorExibido)
        {
            linha.Erro = "O valor recebido em <id> é grande demais para ser exibido no relatório.";
            return linha;
        }

        if (quantidadeRecebida.Length > TamanhoMaximoValorExibido)
        {
            linha.IdRecebido = idRecebido;
            linha.Erro = "O valor recebido em <quantidade> é grande demais para ser exibido no relatório.";
            return linha;
        }

        linha.IdRecebido = idRecebido;
        linha.QuantidadeRecebida = quantidadeRecebida;

        if (string.IsNullOrEmpty(idRecebido))
        {
            linha.Erro = "Campo <id> ausente ou vazio.";
            return linha;
        }

        if (!int.TryParse(idRecebido, NumberStyles.Integer, CultureInfo.InvariantCulture, out var produtoId) || produtoId <= 0)
        {
            linha.Erro = $"ID inválido: '{idRecebido}' não é um número inteiro positivo.";
            return linha;
        }

        linha.ProdutoId = produtoId;

        if (string.IsNullOrEmpty(quantidadeRecebida))
        {
            linha.Erro = "Campo <quantidade> ausente ou vazio.";
            return linha;
        }

        if (!int.TryParse(quantidadeRecebida, NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantidade) || quantidade <= 0)
        {
            linha.Erro = $"Quantidade inválida: '{quantidadeRecebida}' não é um número inteiro positivo.";
            return linha;
        }

        quantidadesPorProduto.TryGetValue(produtoId, out var acumulado);
        acumulado += quantidade;

        quantidadesPorProduto[produtoId] = acumulado;
        linha.Quantidade = quantidade;
        linha.Sucesso = true;
        return linha;
    }

    private static ReposicaoEstoqueResultadoDTO LoteInvalido(List<ReposicaoEstoqueLinhaDTO> linhas)
    {
        return new ReposicaoEstoqueResultadoDTO
        {
            Sucesso = false,
            Erro = "A reposição não foi aplicada: uma ou mais linhas do arquivo são inválidas.",
            TotalLinhas = linhas.Count,
            LinhasProcessadas = 0,
            Linhas = linhas
        };
    }

    private static ReposicaoEstoqueResultadoDTO FalhaGeral(string erro)
    {
        return new ReposicaoEstoqueResultadoDTO
        {
            Sucesso = false,
            Erro = erro,
            TotalLinhas = 0,
            LinhasProcessadas = 0,
            Linhas = []
        };
    }
}
