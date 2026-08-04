namespace backend.DTOs.Usuario;
public class CadastroRequest
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string ConfirmarSenha { get; set; } = string.Empty;
    public string? Perfil { get; set; } = null; 

    public string? HashSenha { get; set; } = null;
}
