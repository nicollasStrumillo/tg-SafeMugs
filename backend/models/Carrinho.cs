using backend.models.Enums;

namespace backend.models;
public class Carrinho
{
    public int Id { get; set; }
    public StatusCarrinho Status { get; set; }
    public DateTime DtCadastro { get; set; }
    public DateTime DtAtualizacao { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public ICollection<ItemCarrinho> Itens { get; set; } = [];
}
