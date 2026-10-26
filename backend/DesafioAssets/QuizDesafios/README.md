# SafeMugs - Quizzes
Um Quiz, no SafeMugs, é uma dinâmica didática que oferece ao usuário o desafio de encontrar a(s) linha(s) responsável(is) pela vulnerabilidade a qual aquele Quiz pertence. 
Os "metadados" de um Quiz são: as linhas corretas — a resposta esperada do cliente; as linhas neutras –– não farão diferença no resultado da resposta; a linguagem do código do Quiz (c#, typescript ou html); de qual Desafio é o Quiz. 

## Criação de um Quiz
Todo o conteúdo dos Quizzes fica na pasta `backend\DesafioAssets\QuizDesafios`.

Cada Desafio deve possuir sua própria pasta, com o respectivo nome do Enum do desafio. 
(vide backend\models\Enums\DesafiosEnum.cs)

Dentro de cada pasta devem haver 3 arquivos:
- Quiz.{extensao}.txt: O escopo do Quiz.
- Seguro.{extensao}.txt: Exemplo de código sem a vulnerabilidade
- MensagemSeguro.txt: Mensagem de sucesso que o usuário recebe após resolver um desafio

## Escopo do Quiz
O escopo do Quiz é o próprio código onde o usuário terá de identificar o trecho responsável pela vulnerabilidade. 

Para criar um arquivo, utilizar o seguinte padrão: `{EnumDesafio}\\Quiz.{extensão arquivo}.txt`

`.txt pra IDE não reclamar...`

Por exemplo, o Quiz criado para o desafio "Login como Admin":

\DesafioAssets\QuizDesafios\LoginAdmin\Quiz.cs.txt:
```
public async Task<LoginResponse?> RealizarLoginAsync(LoginRequest request)
{
    var sql = $@"SELECT // neutra
                u.Id AS UsuarioId, u.NomeCompleto, u.Email, // neutra
                p.Nome AS Perfil // neutra
                FROM usuarios u INNER JOIN perfis p ON u.PerfilId = p.Id // neutra
                WHERE u.Email = '{request.Email}' AND u.HashSenha = '{request.HashSenha}' AND u.Ativo = 1 limit 1;"; // correta
    try 
    { 
        var resposta = await _context.Database 
            .SqlQueryRaw<LoginResponse>(sql) // neutra
            .ToListAsync(); 
        return resposta.FirstOrDefault(); 
    } 
    catch (MySqlConnector.MySqlException ex)
    {
        throw new InvalidOperationException($"MYSQL_ERROR: {ex.Message});
    }
}
```

### Padrão de demarcação de metadados
Perceba os comentários:  "// neutra" e "// correta" posicionados no final de linhas selecionadas
- "// correta": significa que aquela linha é esperada como a resposta do Quiz, ou seja, é uma das linhas que causam a vulnerabilidade em questão. Mais de uma linha pode ser demarcada como "correta"; a resposta de um usuário só é considera como *certa* quando ela possui **todas** as linhas corretas selecionadas. 

- "// neutra": significa que aquela linha pode ser desconsiderada da resposta. Caso ela apareça entre as linhas selecionadas pelo usuário, isso não tornará a sua resposta *errada*. 

- Para todas as demais linhas sem nenhum demarcador: se o usuário selecionar qualquer uma delas, sua resposta é imediatamente considerada como *errada*.

Estes demarcadores não são apenas estéticos. O responsável por montar cada entidade `QuizDesafio` será capaz de resgatar estes demarcadores e automaticamente identificar oque seria considerada uma resposta *certa* ou *errada* para o Quiz. Ou seja, eles são **essenciais**; um Escopo sem **ao menos um** **"//correta"** é um escopo incompleto.


## Código Seguro
Este será o código exibido ao usuário como exemplo de uma implementação sem a vulnerabilidade. 

Para criar um arquivo, utilizar o seguinte padrão: `{EnumDesafio}\\Seguro.{extensão arquivo}.txt`

Por exemplo, o Código Seguro criado para o desafio "Login como Admin":

\DesafioAssets\QuizDesafios\LoginAdmin\Seguro.cs.txt:
```
public async Task<LoginResponse?> RealizarLoginAsync(LoginRequest request)
{
    const string sql = @"
        SELECT
            u.Id AS UsuarioId,
            u.NomeCompleto,
            u.Email,
            p.Nome AS Perfil
        FROM usuarios u
        INNER JOIN perfis p ON u.PerfilId = p.Id
        WHERE u.Email = @email
          AND u.HashSenha = @hashSenha
          AND u.Ativo = 1
        LIMIT 1;";

    try
    {
        var resposta = await _context.Database
            .SqlQueryRaw<LoginResponse>(
                sql,
                new MySqlConnector.MySqlParameter("@email", request.Email),
                new MySqlConnector.MySqlParameter("@hashSenha", request.HashSenha))
            .ToListAsync();

        return resposta.FirstOrDefault();
    }
    catch (MySqlConnector.MySqlException ex)
    {
        throw new InvalidOperationException(
            $"MYSQL_ERROR: {ex.Message}", ex);
    }
}
```
Perceba que ele não é mais vulnerável a SQLInjection


### Mensagem de Quiz Solucionado
É a mensagem que o usuário vai receber após resolver um Quiz. Ela pode servir de apresentação ao código sem a vulnerabilidade — explicar porque aquele é um exemplo seguro.

Para criar um arquivo, utilizar o seguinte padrão: `{EnumDesafio}\\MensagemSeguro.txt`

Por exemplo, a mensagem criada para o desafio "Login como Admin":

\DesafioAssets\QuizDesafios\LoginAdmin\MensagemSeguro.txt:
```
"Toda ferramenta de acesso a banco de dados oferece uma forma segura de enviar parâmetros para uma consulta SQL, evitando que eles sejam concatenados diretamente na string da consulta."
```
