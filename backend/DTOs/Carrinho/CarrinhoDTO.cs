using backend.DTOs.Usuario;

namespace backend.DTOs.Carrinho;

public class CarrinhoDTO
{
    public int Id { get; set; }
    public string Status { get; set; } = null!;
    public decimal Total { get; set; }

    public UsuarioDetalhesDTO Usuario { get; set; } = null!;

    public List<ItemCarrinhoDTO> Itens { get; set; } = new List<ItemCarrinhoDTO>();
    
}
