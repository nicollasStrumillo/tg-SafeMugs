namespace backend.models;

public class TokenRedefinicaoSenha
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime ExpiraEm { get; set; }
    public DateTime? UsadoEm { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
