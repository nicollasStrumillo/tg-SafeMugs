using backend.DTOs.Usuario;

namespace backend.Services.Interfaces
{
    public interface ISenhaService
    {
        Task TrocarSenhaAsync(ResetSenhaRequestDto dto);
    }
}