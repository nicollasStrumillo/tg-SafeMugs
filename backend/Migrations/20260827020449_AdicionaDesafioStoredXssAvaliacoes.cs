using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDesafioStoredXssAvaliacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Nome",
                value: "Stored XSS em Catalogo");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 19, "StoredXSS", "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página de avaliações", "Na página de avaliações do produto, o e-mail do autor de cada avaliação é renderizado diretamente no HTML sem sanitização. O e-mail é um dado persistido da conta do usuário e exibido a qualquer visitante que abra as avaliações daquele produto. Ao interceptar a requisição de cadastro e registrar um usuário com o e-mail |<iframe src=\"javascript:alert(`XSS`)\">|, depois publicar uma avaliação com essa conta, o payload fica armazenado no banco e é interpretado no navegador de quem visualiza a página em |/avaliacoes-produto?nome=...|. Diferente do Reflected XSS, aqui o conteúdo malicioso persiste no servidor e atinge múltiplas vítimas, caracterizando Stored XSS via campo de e-mail.", 4, "Stored XSS em Avaliações", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 41, 19, 1, "O campo vulnerável não é o comentário da avaliação" },
                    { 42, 19, 2, "O dado que foi mal sanitizado faz parte do cadastro de usuários" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Nome",
                value: "Stored XSS");
        }
    }
}
