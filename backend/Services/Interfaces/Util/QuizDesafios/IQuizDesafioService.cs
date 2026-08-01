using backend.models.Enums;
using backend.Services.Implementations.Util.QuizDesafios;

namespace backend.Services.Interfaces.Util.QuizDesafios;

public interface IQuizDesafioService
{
    Task MontarDicionario();
    QuizDesafio? GetQuizDesafio(DesafiosEnum desafio);
    IEnumerable<KeyValuePair<DesafiosEnum, QuizDesafio>> GetAllQuizDesafio();
    bool TrySolveQuizDesafio(DesafiosEnum desafio, int[] linhasSelecionadas, out string mensagem, bool isRestore = false);
}
