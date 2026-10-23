namespace backend.DTOs.Produto;

public class ReposicaoEstoqueResultadoDTO
{
    public bool Sucesso { get; set; }

    public string? Erro { get; set; }

    public int TotalLinhas { get; set; }
    public int LinhasProcessadas { get; set; }

    public List<ReposicaoEstoqueLinhaDTO> Linhas { get; set; } = [];
}
