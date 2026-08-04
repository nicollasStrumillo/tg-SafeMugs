using backend.DTOs.Usuario;

namespace backend.Services.Interfaces;

public interface IUsuarioService
{
    Task CadastrarUsuarioAsync(CadastroRequest request);
    Task<AuthTokenResponse?> RealizarLoginAsync(LoginRequest request);
    Task<UsuarioDetalhesDTO> ObterUsuarioDetalhesAsync(int usuarioId);
    Task<AuthTokenResponse?> EditarUsuarioAsync(EditarUsuarioRequest request);
    Task<AuthTokenResponse?> UploadFotoPerfilAsync(IFormFile foto);
    Task<AuthTokenResponse?> UploadFotoPerfilUrlAsync(UploadFotoPerfilUrlRequest request);
    Task MudarSenhaAsync(MudarSenhaRequest request);
    Task DesativarUsuarioAsync(int usuarioId);
}
