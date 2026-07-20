namespace backend.DTOs.Produto;

public class ComentarioRequest
{
    public string? NomeCompleto { get; set; }
    public string Comentario { get; set; } = string.Empty;
}
