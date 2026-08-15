namespace backend.DTOs.Carrinho;
public class RemoverProdutoCarrinhoRequest
{
    public int UsuarioId { get; set; }  
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }    
}
