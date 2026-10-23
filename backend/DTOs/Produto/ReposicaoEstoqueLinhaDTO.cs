namespace backend.DTOs.Produto;

public class ReposicaoEstoqueLinhaDTO
{
    public bool Sucesso { get; set; }

    public string IdRecebido { get; set; } = string.Empty;
    public string QuantidadeRecebida { get; set; } = string.Empty;

    public int? ProdutoId { get; set; }
    public string? ProdutoNome { get; set; }
    public int? Quantidade { get; set; }

    public int? EstoqueAnterior { get; set; }
    public int? EstoqueNovo { get; set; }

    public string? Erro { get; set; }
}
