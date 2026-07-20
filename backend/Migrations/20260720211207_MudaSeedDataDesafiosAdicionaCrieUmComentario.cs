using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MudaSeedDataDesafiosAdicionaCrieUmComentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 23);

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
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 13);

            // Order matters to avoid unique index violations.
            // Desafios IX_desafios_Nome: Id=8 & Id=9 relinquish names first, then Id=6 & Id=7 take them.
            // Dicas IX_dicas_desafio_DesafioId_NrDica: Id=21 (10,1) moves to (10,3) before Id=19 takes (10,1).

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "BrokenAuthentication", "Escreva um comentário que não pertence ao seu usuário.", 2, "Crie um comentário por outro usuário" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "Outros", "Localize a página de score-board.", 1, "Encontrar a Score-Board" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Categoria", "Descricao", "Nome" },
                values: new object[] { "IDOR", "Edite um comentário que não pertence ao seu usuário.", "Altere o comentário de outro usuário" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "StoredXSS", "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de Stored XSS na página do catalogo", 3, "Stored XSS" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "SecurityMisconfiguration", "Provoque um erro que o retorno da API não trata corretamente.", 1, "Tratamento de Erro" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 13,
                column: "Texto",
                value: "Observe o corpo da requisição de editar comentário.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 14,
                column: "Texto",
                value: "Intercepte a requisição de editar comentário para mudar o seu payload.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 15,
                column: "Texto",
                value: "O desafio está na seção de detalhes do produto.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 16,
                column: "Texto",
                value: "Comentários podem ser renderizados sem sanitização.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 17,
                column: "Texto",
                value: "O payload enviado ao backend pode estar transmitindo dados de forma insegura.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 18,
                column: "Texto",
                value: "Tente interceptar e modificar a requisição.");

            // Id=21 relinquishes (DesafioId=10, NrDica=1) before Id=19 takes (10,1)
            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 3, "Tente quebrar a consulta SQL realizada a partir da tela de Login." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DesafioId", "Texto" },
                values: new object[] { 10, "Este desafio pode ser resolvido a partir de diferentes telas." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DesafioId", "Texto" },
                values: new object[] { 10, "Tente inserir valores inesperados em formulários que possam provocar um erro no backend." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Categoria", "Descricao", "Nome" },
                values: new object[] { "SqlInjection", "Use a busca do catálogo para injetar uma consulta.", "Buscar por SQL Injection" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "ReflectedXSS", "Explore a busca do catálogo com payload de XSS refletido.", 2, "Buscar com script" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "ParameterTampering", "Edite um comentário que não pertence ao seu usuário.", 3, "Altere o comentário de outro usuário" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "StoredXSS", "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de Stored XSS na página do catalogo", 3, "Stored XSS" });

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Categoria", "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "IDOR", "Escreva um comentário que não pertence ao seu usuário.", 3, "Criar um comentário por outro usuário" });

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "Dificuldade", "Nome", "Resolvido", "UrlMitigacao" },
                values: new object[,]
                {
                    { 11, "BrokenAuthentication", "Explore o fluxo de esqueci minha senha sem proteção suficiente.", 3, "Recuperar senha insegura", false, "url_placeholder" },
                    { 12, "Outros", "Localize a página de score-board.", 1, "Encontrar a Score-Board", false, "url_placeholder" },
                    { 13, "SecurityMisconfiguration", "Provoque um erro que o retorno da API não trata corretamente.", 1, "Tratamento de Erro", false, "url_placeholder" }
                });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 13,
                column: "Texto",
                value: "A busca do catálogo é a superfície de ataque.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 14,
                column: "Texto",
                value: "O nome do desafio aponta para SQL Injection.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 15,
                column: "Texto",
                value: "A busca reflete sua entrada na interface de resposta.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 16,
                column: "Texto",
                value: "Teste inserir um payload malicioso no termo de pesquisa.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 17,
                column: "Texto",
                value: "Observe o corpo da requisição de editar comentário.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 18,
                column: "Texto",
                value: "Intercepte a requisição de editar comentário para mudar o seu payload.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DesafioId", "Texto" },
                values: new object[] { 9, "O desafio está na seção de detalhes do produto." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DesafioId", "Texto" },
                values: new object[] { 9, "Comentários podem ser renderizados sem sanitização." });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "NrDica", "Texto" },
                values: new object[] { 1, "O payload enviado ao backend pode estar transmitindo dados de forma insegura." });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 22, 10, 2, "Tente interceptar e modificar a requisição." },
                    { 23, 11, 1, "Dica 1 placeholder" },
                    { 24, 11, 2, "Dica 2 placeholder" },
                    { 25, 13, 1, "Este desafio pode ser resolvido a partir de diferentes telas." },
                    { 26, 13, 2, "Tente inserir valores inesperados em formulários que possam provocar um erro no backend." },
                    { 27, 13, 3, "Tente quebrar a consulta SQL realizada a partir da tela de Login." }
                });
        }
    }
}
