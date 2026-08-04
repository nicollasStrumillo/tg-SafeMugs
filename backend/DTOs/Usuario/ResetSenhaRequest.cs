using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Usuario;

public class ResetSenhaRequestDto
{
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string NovaSenha { get; set; } = string.Empty;

}
