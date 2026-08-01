using backend.models.Enums;
using backend.Services.Interfaces.Util.QuizDesafios;
using System.Text.RegularExpressions;

namespace backend.Services.Implementations.Util.QuizDesafios;

public class QuizDesafioService : IQuizDesafioService
{
    private readonly Dictionary<DesafiosEnum, QuizDesafio> _quizDesafios = [];

    public async Task MontarDicionario()
    {
        var pastaQuiz = Path.Combine(AppContext.BaseDirectory, "QuizDesafios");
        if (!Directory.Exists(pastaQuiz))
            throw new InvalidOperationException($"Pasta de QuizDesafios não encontrada em: {pastaQuiz}");

        // regex: \s*//\s*(correta|neutra)\s*$ -> detecta // correta ou // neutra no final da linha
        // \s*       -> zero ou mais espaços em branco
        // //        -> literalmente dois caracteres de barra
        // (correta|neutra) -> captura "correta" ou "neutra"
        // $         -> fim da linha
        // RegexOptions.IgnoreCase  = ignora maiúsculas/minúsculas
        var regexMarker = new Regex(@"\s*//\s*(correta|neutra)\s*$", RegexOptions.IgnoreCase);

        foreach (var pasta in Directory.EnumerateDirectories(pastaQuiz))
        {
            var nomePasta = Path.GetFileName(pasta); // Ex: "LoginAdmin"

            if (!Enum.TryParse<DesafiosEnum>(nomePasta, ignoreCase: true, out var enumDesafio))
                throw new InvalidOperationException(
                    $"Pasta de QuizDesafio '{nomePasta}' não casa nenhum valor de {nameof(DesafiosEnum)}. " +
                    $"Verifique o nome da pasta ou adicione a entrada no enum.");

            string? arquivoQuiz = null;
            string? arquivoSeguro = null;
            string? arquivoMensagem = null;

            foreach (var arquivo in Directory.EnumerateFiles(pasta))
            {
                var nome = Path.GetFileName(arquivo);

                if (nome.StartsWith("Quiz.", StringComparison.OrdinalIgnoreCase) && nome.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    arquivoQuiz = arquivo;
                else if (nome.StartsWith("Seguro.", StringComparison.OrdinalIgnoreCase) && nome.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    arquivoSeguro = arquivo;
                else if (nome.Equals("MensagemSeguro.txt", StringComparison.OrdinalIgnoreCase))
                    arquivoMensagem = arquivo;
            }

            if (arquivoQuiz is null || arquivoSeguro is null || arquivoMensagem is null)
                throw new InvalidOperationException(
                    $"Pasta de QuizDesafio '{nomePasta}' deve conter 'Quiz.<ext>.txt', 'Seguro.<ext>.txt' e 'MensagemSeguro.txt'. " +
                    $"Encontrado: Quiz={(arquivoQuiz is null ? "ausente" : "presente")}, " +
                    $"Seguro={(arquivoSeguro is null ? "ausente" : "presente")}, " +
                    $"Mensagem={(arquivoMensagem is null ? "ausente" : "presente")}.");

            var nomeSemTxt = Path.GetFileNameWithoutExtension(arquivoQuiz); // "Quiz.cs"
            var extReal = Path.GetExtension(nomeSemTxt);                    // ".cs"
            var linguagem = extReal.ToLowerInvariant() switch
            {
                ".cs"   => "CSharp",
                ".ts"   => "TypeScript",
                ".html" => "Html",
                _ => throw new InvalidOperationException(
                    $"QuizDesafio '{nomePasta}/Quiz{extReal}.txt': extensão '{extReal}' não suportada. " +
                    $"Esperado .cs, .ts ou .html (sujeito ao sufixo .txt).")
            };

            var linhas = await File.ReadAllLinesAsync(arquivoQuiz);

            var quiz = new QuizDesafio
            {
                NomeDesafio = enumDesafio.GetNomeDisplay(),
                Linguagem = linguagem,
                LinhasQuiz = new List<string>(),
                LinhasCorretas = new List<int>(),
                LinhasNeutras = new List<int>(),
                LinhasCodigoSeguro = new List<string>(),
                Resolvido = false
            };

            for (var i = 0; i < linhas.Length; i++)
            {
                var linha = linhas[i];

                var match = regexMarker.Match(linha); // verifica se a linha contém o marcador "// correta" ou "// neutra" no final
                if (match.Success)
                {
                    var marcador = match.Groups[1].Value.ToLowerInvariant(); // pega o grupo 1 da regex, que é "correta" ou "neutra"
                    linha = linha[..match.Index];

                    var numLinha = i + 1; // i + 1 pra pegar a numeração da linha (começa em 1, não em 0)
                    if (marcador == "correta")
                        quiz.LinhasCorretas.Add(numLinha);
                    else
                        quiz.LinhasNeutras.Add(numLinha);
                }

                quiz.LinhasQuiz.Add(linha);
            }

            var linhasSeguro = await File.ReadAllLinesAsync(arquivoSeguro);
            quiz.LinhasCodigoSeguro = linhasSeguro.ToList();

            quiz.MensagemSeguro = await File.ReadAllTextAsync(arquivoMensagem);

            _quizDesafios[enumDesafio] = quiz;
        }
    }

    public QuizDesafio? GetQuizDesafio(DesafiosEnum desafio)
    {
        _quizDesafios.TryGetValue(desafio, out var quiz);

        return quiz;
    }

    public IEnumerable<KeyValuePair<DesafiosEnum, QuizDesafio>> GetAllQuizDesafio()
    {
        return _quizDesafios;
    }

    public bool TrySolveQuizDesafio(DesafiosEnum desafio, int[] linhasSelecionadas, out string mensagem, bool isRestore = false)
    {
        if (!_quizDesafios.TryGetValue(desafio, out var quiz))
        {
            mensagem = "DesafioQuiz não encontrado.";
            return false;
        }

        if (quiz.Resolvido)
        {
            mensagem = "Quiz já resolvido.";
            return false;
        }

        // Para os casos de restauração, não é necessário validar as linhas selecionadas, apenas marcar como resolvido.
        if (isRestore)
        {
            quiz.Resolvido = true;
            mensagem = "Quiz resolvido via restauração.";
            return true;
        }

        var setSelecionadas = linhasSelecionadas.ToHashSet(); // tira duplicatas
        var invalidas = setSelecionadas.Except(quiz.LinhasCorretas).Except(quiz.LinhasNeutras); // Procura linhas invalidas
        
        if (invalidas.Any()) { 
            mensagem = "Linha inválida selecionada!"; 
            return false; 
        }
    
        if (!quiz.LinhasCorretas.Any(setSelecionadas.Contains)) {
            mensagem = "Nenhuma linha vulnerável foi selecionada!";
            return false;
        }

        if (!quiz.LinhasCorretas.All(setSelecionadas.Contains)) { 
            mensagem = "Nem todas linhas vulneráveis foram selecionadas!"; 
            return false; 
        }

        mensagem = "Sucesso!";
        quiz.Resolvido = true;
        return true;
    }
}
