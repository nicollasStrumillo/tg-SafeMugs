using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteraSeedDataDesafios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descricao", "Dificuldade" },
                values: new object[] { "Acesse uma conta administrativa.", 2 });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Descricao",
                value: "Crie uma conta de administrador.");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "Dificuldade", "Nome", "Resolvido", "UrlMitigacao" },
                values: new object[] { 13, "SecurityMisconfiguration", "Provoque um erro que o retorno da API não trata corretamente.", 1, "Tratamento de Erro", false, "url_placeholder" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 1,
                column: "Texto",
                value: "Utilize SQL Injection para provocar um erro e observe a resposta da API.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 2,
                column: "Texto",
                value: "Tente identificar o e-mail de um administrador para fazer um ataque direcionado.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 3,
                column: "Texto",
                value: "Também é possível resolver utilizando outra coluna da tabela Usuários que não seja o e-mail.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 10,
                column: "Texto",
                value: "Identifique as colunas que compõem um usuário.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 11,
                column: "Texto",
                value: "Observar a resposta de uma requisição de login bem-sucedida é uma maneira de identificar as colunas que compõem um usuário.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 5, 3, "Você pode interceptar a requisição de cadastro e mudar o seu corpo." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 1, "A busca do catálogo é a superfície de ataque." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 6, 2, "O nome do desafio aponta para SQL Injection." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 1, "A busca reflete sua entrada na interface de resposta." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 7, 2, "Teste inserir um payload malicioso no termo de pesquisa." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 1, "Filtros e ordenação costumam vir por query string." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 8, 2, "O backend pode estar ordenando por qualquer parâmetro que ele receber." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 1, "O desafio está na página de detalhes do produto." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 9, 2, "Comentários podem ser renderizados sem sanitização." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 1, "O payload enviado ao backend pode estar transmitindo dados de forma insegura." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 10, 2, "Tente interceptar e modificar a requisição." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 1, "Dica 1 placeholder" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 24, 11, 2, "Dica 2 placeholder" },
                    { 25, 13, 1, "Este desafio pode ser resolvido a partir de diferentes telas." },
                    { 26, 13, 2, "Tente inserir valores inesperados em formulários que possam provocar um erro no backend." },
                    { 27, 13, 3, "Tente quebrar a consulta SQL realizada a partir da tela de Login." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descricao", "Dificuldade" },
                values: new object[] { "Acesse a conta administrativa usando o fluxo de login.", 3 });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Descricao",
                value: "Altere os dados enviados antes que eles cheguem ao servidor e crie uma conta de administrador.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 1,
                column: "Texto",
                value: "Primeiro, identifique qual é o e-mail do administrador.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 2,
                column: "Texto",
                value: "A falha está no fluxo de autenticação.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 3,
                column: "Texto",
                value: "Teste SQL Injection em entradas no campo de e-mail ou senha.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 10,
                column: "Texto",
                value: "Primeiro, identifique um campo que compõe um usuário mas não devia ser enviado pelo formulário de cadastro.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 11,
                column: "Texto",
                value: "O payload pode ser alterado antes de chegar ao servidor.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 6, 1, "A busca do catálogo é a superfície de ataque." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 2, "O nome do desafio aponta para SQL Injection." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 7, 1, "A busca reflete sua entrada na interface de resposta." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 2, "Teste inserir um payload malicioso no termo de pesquisa." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 8, 1, "Filtros e ordenação costumam vir por query string." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 2, "O backend pode estar ordenando por qualquer parâmetro que ele receber." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 9, 1, "O desafio está na página de detalhes do produto." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 2, "Comentários podem ser renderizados sem sanitização." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 10, 1, "O payload enviado ao backend pode estar transmitindo dados de forma insegura." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 2, "Tente interceptar e modificar a requisição." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DesafioId", "NrDica", "Texto" },
                values: new object[] { 11, 1, "Dica 1 placeholder" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 2, "Dica 2 placeholder" });
        }
    }
}
