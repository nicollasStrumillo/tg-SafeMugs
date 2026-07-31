namespace backend.Utils.QuizDesafios;

public class QuizDesafio
{
    public string NomeDesafio { get; set; } = string.Empty;
    public string Linguagem { get; set; } = string.Empty;
    public bool Resolvido { get; set; } = false;

    // Quiz
    public List<string> LinhasQuiz { get; set; } = new List<string>();
    public List<int> LinhasCorretas { get; set; } = new List<int>();
    public List<int> LinhasNeutras { get; set; } = new List<int>();
    
    // Codigo Seguro
    public List<string> LinhasCodigoSeguro { get; set; } = new List<string>();
    public string MensagemSeguro { get; set; } = string.Empty;
}
