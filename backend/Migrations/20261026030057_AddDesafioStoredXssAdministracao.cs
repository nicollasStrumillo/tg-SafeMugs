using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioStoredXssAdministracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 30, "StoredXSS", "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página de Administração.", "Stored XSS ocorre quando conteúdo controlado pelo usuário é armazenado e posteriormente interpretado como HTML. Neste fluxo, o e-mail associado ao autor de uma avaliação é persistido na conta e exibido na listagem administrativa. Como esse valor é tratado como HTML, conteúdo malicioso pode ser executado no navegador de quem visualiza a lista. Dados persistidos devem ser tratados como não confiáveis e renderizados com codificação adequada ao contexto.", 4, "Stored XSS em Administração", false, "/administracao" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 59, 30, 1, "A listagem de avaliações mostra outros dados do autor além do comentário." },
                    { 60, 30, 2, "O e-mail do autor vem do cadastro e é exibido na listagem; observe como o navegador interpreta esse valor." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}
