namespace backend.DTOs.Pedido;

public class PedidoResumoDTO
{
    public int Id { get; set; }
    public string NumeroPedido { get; set; } = null!;
    public decimal ValorTotal { get; set; }
    public int QuantidadeItens { get; set; }
    public DateTime DataPedido { get; set; }
    public int UsuarioId { get; set; }  
}
