namespace backend.DTOs.Carrinho;
public class AdicionarProdutoCarrinhoRequest
{
    public int UsuarioId { get; set; }  
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }    
}
