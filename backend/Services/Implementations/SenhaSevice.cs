using backend.DTOs.Auth;
using backend.Helpers;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations
{
    public class SenhaService : ISenhaService
{
    private readonly IUsuarioRepository usuarioRepository;

    
    public SenhaService(IUsuarioRepository usuarioRepository)
    {
        this.usuarioRepository = usuarioRepository;
    }

    public async Task TrocarSenhaAsync(ResetSenhaRequestDto dto)
    {
        var usuario = await usuarioRepository.BuscaPorEmailAsync(dto.Email);

        if (usuario == null)
            throw new Exception("Usuário não encontrado.");

        usuario.HashSenha = HashHelper.GerarMD5(dto.NovaSenha);

        await usuarioRepository.AtualizarAsync(usuario);
    }
}
}