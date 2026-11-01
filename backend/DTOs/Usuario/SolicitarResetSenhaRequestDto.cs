using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Usuario;

public class SolicitarResetSenhaRequestDto
{
    public string Email { get; set; } = string.Empty;
}
