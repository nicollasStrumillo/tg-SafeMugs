namespace backend.DTOs.Produto;

public class ComentarioRequest
{
    public int? UsuarioId { get; set; }
    public string Comentario { get; set; } = string.Empty;
}
