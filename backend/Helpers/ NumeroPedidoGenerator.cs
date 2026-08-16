namespace backend.Helpers;
public static class NumeroPedidoGenerator
{
    // Essa classe é responsável por gerar NumeroPedido's que nunca se repetem utilizando o ID do pedido como entrada
    // Uma numero de pedido tem essa forma:
    // AA00000 
    // Sendo: A -> A-Z (26 possibilidades); 0 -> 0 - 9 (10 possibilidades)

    // A ideia é utilizar uma *permutação afim modular* -> f(x) = ax + b (mod N)
    // x: A entrada (ID do pedido)
    // a: coeficiente multiplicativo 
    // b: off-set
    // N: módulo (número de possibilidades)

    // Para garantir que a função seja bijetora, a e N devem ser coprimos
    // Bijetora: significa que cada entrada x gera uma saída única f(x) e vice-versa, garantindo que não haja colisões
    // coprimos: mdc(a, N) = 1 -> quando dois números possuem o maior divisor comum (mdc) igual a 1.  

    //N = numero de possibilidades = 26 x 26 x 10 x 10 x 10 x 10 x 10:
    private const long N = 67_600_000;

    // a = N-1:
    private const long a = 67_599_999; // mdc(N-1, N) = 1 -> a é coprimo de N 

    // off-set: um número qualquer que faz o deslocamento do resultado 
    private const long b = 27_300_255; // off-set

    public static string GerarNumeroPedido(int pedidoId)
    {   
        // Para não causar repetições, x tem que estar dentro do conjunto 1..N
        if (pedidoId <= 0 || pedidoId > N) 
            throw new ArgumentOutOfRangeException(nameof(pedidoId));

        //        f(x) =  a * x        + b (mod N)
        long resultado = (a * pedidoId + b) % N;

        long parteAlfa = resultado / 100_000; // 0..675
        long parteNum = resultado % 100_000; // 0..99999

        // Codifica pra base 26
        char primeiraLetra = (char)('A' + parteAlfa / 26);
        char segundaLetra = (char)('A' + parteAlfa % 26);

        string numeroPedido = $"{primeiraLetra}{segundaLetra}{parteNum:D5}";
        
        return numeroPedido;
    }
}
