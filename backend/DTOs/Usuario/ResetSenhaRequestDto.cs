namespace backend.DTOs.Usuario;

public class ResetSenhaRequestDto
{
    public string NovaSenha { get; set; } = string.Empty;

    public string ConfirmarSenha { get; set; } = string.Empty;
}
