namespace backend.DTOs.Produto;

public class ProdutoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public CategoriaProdutoDTO CategoriaProduto { get; set; } = null!;

    public string UrlImagemProduto { get; set; } = string.Empty;
}
