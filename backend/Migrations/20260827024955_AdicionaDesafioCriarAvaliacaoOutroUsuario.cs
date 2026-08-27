using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDesafioCriarAvaliacaoOutroUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descricao",
                value: "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de DOM XSS na página do catalogo.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Descricao",
                value: "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página do catalogo.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                column: "Categoria",
                value: "IDOR");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 19,
                column: "Descricao",
                value: "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página de avaliações.");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 20, "IDOR", "Crie uma avaliação em nome de outro usuário.", "Ao criar uma avaliação, o cliente envia no corpo da requisição o identificador do autor (usuarioId) junto com nota e comentario para POST /api/produtos/avaliacoes/{produtoId}. O servidor apenas verifica se o usuário informado existe e o vincula à avaliação, sem conferir se corresponde ao usuário autenticado. Interceptando a requisição disparada pelo diálogo de avaliação e trocando usuarioId pelo de outro usuário, é possível registrar uma avaliação em nome de outra pessoa.", 2, "Avaliação ilegítima", false });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 41,
                column: "Texto",
                value: "O campo vulnerável não é o comentário da avaliação.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 42,
                column: "Texto",
                value: "O dado que foi mal sanitizado faz parte do cadastro de usuários.");

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[] { 43, 20, 1, "O autor da avaliação vem do corpo da requisição, não da sessão autenticada." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descricao",
                value: "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de DOM XSS na página do catalogo");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Descricao",
                value: "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página do catalogo");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                column: "Categoria",
                value: "BrokenAuthentication");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 19,
                column: "Descricao",
                value: "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página de avaliações");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 41,
                column: "Texto",
                value: "O campo vulnerável não é o comentário da avaliação");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 42,
                column: "Texto",
                value: "O dado que foi mal sanitizado faz parte do cadastro de usuários");
        }
    }
}
