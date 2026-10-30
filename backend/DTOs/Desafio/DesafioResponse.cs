namespace backend.DTOs.Desafio;

public class DesafioResponse
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;
    public string CategoriaId { get; set; } = string.Empty;

    public int Dificuldade { get; set; }
    
    public string? Rota { get; set; }
    
    public bool Resolvido { get; set; }

    public bool PossuiQuiz { get; set; } = false;
    public bool QuizResolvido { get; set; } = false;

    public bool IsRestored { get; set; } = false;

    public List<DicaDesafioDTO> DicasDesafio { get; set; } = new List<DicaDesafioDTO>();
}
