namespace backend.models;
public class Avaliacao
{
    public int Id { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime DtCadastro { get; set; }
    public DateTime DtAtualizacao { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

}
