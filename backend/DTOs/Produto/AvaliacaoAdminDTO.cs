namespace backend.DTOs.Produto;

public class AvaliacaoAdminDTO
{
    public int Id { get; set; }
    public float Nota { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime DtCadastro { get; set; }

    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public string UsuarioEmail { get; set; } = string.Empty;
    public string? UsuarioUrlImagemPerfil { get; set; }
    public bool UsuarioAtivo { get; set; }

    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
}
