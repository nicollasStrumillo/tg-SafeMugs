using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDescricoesDetalhesDesafios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "DescricaoDetalhes",
                value: "O campo de busca do catálogo reflete, no próprio DOM do navegador, o termo digitado sem qualquer tratamento. A entrada do usuário é inserida diretamente no HTML da página, permitindo que tags e atributos perigosos sejam interpretados pelo navegador. Como a manipulação acontece inteiramente no lado do cliente, nenhum dado malicioso precisa trafegar pelo servidor: tudo ocorre no DOM. O payload |<iframe src=\"javascript:alert(`XSS`)\">|, por exemplo, é interpretado ao ser renderizado no catálogo, comprovando uma injeção de HTML tipicamente conhecida como DOM XSS.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                column: "DescricaoDetalhes",
                value: "O fluxo de login do SafeMugs não implementa nenhum mecanismo de antiautomação: não há limite de tentativas por conta, bloqueio temporário, captcha nem atraso entre requisições. Conhecendo um e-mail válido do domínio |@safemugs.com|, é possível automatizar tentativas consecutivas de senha até acertar. As senhas dos usuários comuns são fracas e baseadas em padrões conhecidos (como 'welcome123' ou 'password123'), o que torna viável um ataque usando uma lista de palavras. Sem qualquer controle de velocidade, um script consegue testar milhares de combinações em poucos segundos, obtendo acesso a uma conta legítima.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                column: "DescricaoDetalhes",
                value: "O formulário de cadastro valida os dados apenas no navegador. No servidor, a checagem de formato do e-mail é insuficiente e não impede registros com valores malformados. Ao interceptar a requisição de cadastro e enviar um e-mail em formato inválido (como |ana@@safemugs..com|), o servidor aceita o registro. Isso demonstra que a validação do front-end pode ser facilmente contornada e que o servidor não deve confiar nos dados recebidos, precisando aplicar regras de formato e sanitização por conta própria.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5,
                column: "DescricaoDetalhes",
                value: "O cadastro de um novo usuário é definido, em parte, por um parâmetro que indica o perfil da conta. Embora esse campo não apareça no formulário visível, ele trafega na requisição. Ao interceptar o cadastro e incluir |\"perfil\":\"Administrador\"| no corpo enviado, a conta é criada com privilégios administrativos, sem que o servidor questione o nível solicitado. Confiam-se cegamente nos parâmetros informados pelo cliente, o que permite a escalação de privilégio direta via adulteração de parâmetro.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6,
                column: "DescricaoDetalhes",
                value: "A requisição que atualiza um comentário recebe apenas o identificador do comentário e o novo texto, sem verificar se ele pertence ao usuário autenticado. Ao interceptar a edição e trocar o identificador informado pelo de um comentário alheio, é possível alterá-lo livremente. O servidor não valida a posse do recurso, confiando apenas no parâmetro recebido, o que caracteriza uma Insecure Direct Object Reference (IDOR).");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "DescricaoDetalhes",
                value: "Na tela de detalhes do produto, os comentários deixados pelos usuários são renderizados diretamente no HTML, sem qualquer sanitização. Como o comentário é salvo no banco de dados e exibido a todos que abrem o produto, enviar o payload |<iframe src=\"javascript:alert(`XSS`)\">| como texto do comentário faz com que o código seja armazenado e, posteriormente, interpretado no navegador de qualquer pessoa que visualize a página. Diferente do DOM XSS, aqui o conteúdo malicioso persiste no servidor, atingindo múltiplas vítimas.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                column: "DescricaoDetalhes",
                value: "Todo comentário enviado ao servidor carrega um campo que identifica o autor a ser atribuído a ele. O servidor não valida se esse nome corresponde ao usuário autenticado: ele apenas procura o usuário pelo nome informado e o vincula ao comentário. Interceptando a requisição de criação e alterando o nome pelo de outro usuário (por exemplo |Bruno Costa|), é possível registrar um comentário em nome de outra pessoa, simulando uma identidade indevida.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                column: "DescricaoDetalhes",
                value: "O SafeMugs possui uma página oculta que concentra o acompanhamento dos desafios e o progresso do jogador, sem estar referenciada em nenhum menu ou link visível. Para localizá-la, é preciso explorar nomes de rotas comuns e deduzir a URL por força bruta, ou inspecionar o código javascript da página. O objetivo aqui é treinar a descoberta do domínio sendo testado, prática comum em avaliações de segurança.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10,
                column: "DescricaoDetalhes",
                value: "Quando ocorre um erro inesperado no SafeMugs, a API retorna ao cliente uma resposta contendo o nome da classe da exceção e a mensagem original, frequentemente acompanhada de detalhes internos como nomes de tabelas, consultas SQL e trechos de código. Ao provocar uma falha no backend, é possível extrair dessa resposta informações sensíveis sobre a implementação. Um exemplo é inserir uma aspa simples no e-mail do login (|'|) para quebrar a consulta SQL e forçar uma exceção não tratada, expondo os detalhes técnicos no corpo retornado.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "DescricaoDetalhes",
                value: "O payload |<iframe src=\"javascript:alert(`XSS`)\">| é um exemplo de ataque de DOM XSS. Ele insere um iframe que executa código JavaScript quando a página é carregada. Isso pode ser usado para roubar informações do usuário ou executar ações não autorizadas.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "DescricaoDetalhes",
                value: "O payload |<iframe src=\"javascript:alert(`XSS`)\">| é um exemplo de ataque de Stored XSS. Ele insere um iframe que executa código JavaScript quando a página é carregada. Isso pode ser usado para roubar informações do usuário ou executar ações não autorizadas.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10,
                column: "DescricaoDetalhes",
                value: "placeholder");
        }
    }
}
