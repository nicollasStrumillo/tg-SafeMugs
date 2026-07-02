namespace backend.models;
public class ComentarioProduto
{
    public int Id { get; set; }
    public string Comentario { get; set; } = string.Empty;

    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; } = null;

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;
}
