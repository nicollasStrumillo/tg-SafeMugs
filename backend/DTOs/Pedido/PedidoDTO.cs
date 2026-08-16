using backend.DTOs.Carrinho;
using backend.DTOs.Endereco;
using backend.DTOs.Usuario;

namespace backend.DTOs.Pedido;

public class PedidoDTO
{
    public int Id { get; set; }
    public string NumeroPedido { get; set; } = null!;
    public decimal ValorTotal { get; set; }
    public int QuantidadeItens { get; set; }

    public UsuarioDetalhesDTO Usuario { get; set; } = null!;

    public EnderecoDTO Endereco { get; set; } = null!;
    
    public CarrinhoDTO Carrinho { get; set; } = null!;
}
