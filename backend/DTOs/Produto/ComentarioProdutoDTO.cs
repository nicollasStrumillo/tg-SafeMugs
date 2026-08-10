using backend.DTOs.Usuario;

namespace backend.DTOs.Produto;

public class ComentarioProdutoDTO
{
    public int Id { get; set; }
    public string Comentario { get; set; } = string.Empty;

    public ProdutoDTO? Produto { get; set; }
    public UsuarioDetalhesDTO? Usuario { get; set; }
}
