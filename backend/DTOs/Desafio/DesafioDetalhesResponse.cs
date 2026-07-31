using backend.Utils.QuizDesafios;

namespace backend.DTOs.Desafio;

public class DesafioDetalhesResponse
{
    public int Id { get; set; }
    
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string DescricaoDetalhes { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;
    public string DescricaoCategoria { get; set; } = string.Empty;

    public int Dificuldade { get; set; }
    public bool Resolvido { get; set; }
    
    public bool PossuiQuiz { get; set; } = false;
    public bool QuizResolvido { get; set; } = false;

    public List<DicaDesafioDTO> DicasDesafio { get; set; } = new List<DicaDesafioDTO>();

    public QuizDesafioDTO? QuizDesafio { get; set; } = null;
}
