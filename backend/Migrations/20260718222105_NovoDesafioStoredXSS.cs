using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class NovoDesafioStoredXSS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Descricao", "Nome" },
                values: new object[] { "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de Stored XSS na página do catalogo", "Stored XSS" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 19,
                column: "Texto",
                value: "O desafio está na seção de detalhes do produto.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Descricao", "Nome" },
                values: new object[] { "Insira conteúdo malicioso nos comentários do produto.", "Comentar com HTML" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 19,
                column: "Texto",
                value: "O desafio está na página de detalhes do produto.");
        }
    }
}
