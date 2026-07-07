namespace backend.DTOs.Desafio;

public class DesafioResponse
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public int Dificuldade { get; set; }

    public string UrlMitigacao { get; set; } = string.Empty;

    public List<DicaDesafioDTO> DicasDesafio { get; set; } = new List<DicaDesafioDTO>();
}
