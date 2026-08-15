namespace backend.models;

public class Pedido
{
    public int Id { get; set; }
    public string NumeroPedido { get; set; } = null!;
    public decimal ValorTotal { get; set; }
    public int QuantidadeItens { get; set; }
    
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int EnderecoId { get; set; }
    public Endereco Endereco { get; set; } = null!;

    public int CarrinhoId { get; set; }
    public Carrinho Carrinho { get; set; } = null!;
}
