using backend.DTOs.Auth;

namespace backend.Services.Interfaces
{
    public interface ISenhaService
    {
        Task TrocarSenhaAsync(ResetSenhaRequestDto dto);
    }
}