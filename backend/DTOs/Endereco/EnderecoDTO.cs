namespace backend.DTOs.Endereco;

public class EnderecoDTO
{
    public int Id { get; set; }
    public string Logradouro { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
}
