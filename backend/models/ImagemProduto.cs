namespace backend.models;
public class ImagemProduto
{
    public int Id { get; set; }
    public string UrlImagem { get; set; } = string.Empty;
    public string? Legenda { get; set; }

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;
}
