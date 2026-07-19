namespace backend.DTOs.Produto;

public class EdicaoComentarioRequest
{
    public int ComentarioId { get; set; }
    public string Comentario { get; set; } = string.Empty;
}
