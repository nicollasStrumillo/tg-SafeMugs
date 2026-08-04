using backend.DTOs.Endereco;

namespace backend.DTOs.Usuario;

public class UsuarioDetalhesDTO
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public bool Ativo { get; set; }
    public DateTime DtCadastro { get; set; }
    public DateTime DtAtualizacao { get; set; }
    public string UrlImagemPerfil { get; set; } = string.Empty;

    public string Perfil { get; set; } = null!;

    public EnderecoDTO? Endereco { get; set; }
}
