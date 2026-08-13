using backend.DTOs.Usuario;

namespace backend.DTOs.Produto;

public class AvaliacaoDTO
{
    public int Id { get; set; }
    public float Nota { get; set; }
    public string Comentario { get; set; } = string.Empty;

    public ProdutoDTO? Produto { get; set; }
    public UsuarioDetalhesDTO? Usuario { get; set; }
}
