using backend.DTOs.Produto;

namespace backend.DTOs.Carrinho;

public class ItemCarrinhoDTO
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    
    public decimal PrecoTotal => Quantidade * PrecoUnitario;

    public ProdutoDTO Produto { get; set; } = null!;
}
