namespace backend.DTOs.Usuario;

public class LoginResponse
{
    public int UsuarioId { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public string UrlImagemPerfil { get; set; } = string.Empty;
}
