namespace backend.models;
public class ItemCarrinho
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal SubTotal { get; set; }

    public int CarrinhoId { get; set; }
    public Carrinho Carrinho { get; set; } = null!;

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;
}
