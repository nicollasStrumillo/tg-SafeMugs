using backend.models.Enums;

namespace backend.models;
public class Pedido
{
    public int Id { get; set; }
    public int NumeroPedido { get; set; }
    public StatusPedido Status { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime? DtPagamento { get; set; }
    public DateTime? DtEnviado { get; set; }
    public DateTime? DtEntregue { get; set; }
    public DateTime DtCadastro { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int EnderecoId { get; set; }
    public Endereco Endereco { get; set; } = null!;

    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}
