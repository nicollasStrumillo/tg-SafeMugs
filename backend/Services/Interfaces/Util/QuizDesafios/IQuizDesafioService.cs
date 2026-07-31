using backend.models.Enums;
using backend.Utils.QuizDesafios;

namespace backend.Services.Interfaces.Util.QuizDesafios;

public interface IQuizDesafioService
{
    Task MontarDicionario();
    QuizDesafio? GetQuizDesafio(DesafiosEnum desafio);
    bool TrySolveQuizDesafio(DesafiosEnum desafio, int[] linhasSelecionadas, out string mensagem);
}
