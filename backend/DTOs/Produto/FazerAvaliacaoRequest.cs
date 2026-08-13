namespace backend.DTOs.Produto;

public class FazerAvaliacaoRequest
{
    public int UsuarioId { get; set; }

    public float Nota { get; set; }
    public string Comentario { get; set; } = null!;
}
