using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class NovoDesafioDOMXSS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Categoria", "Descricao", "Nome" },
                values: new object[] { "DomXSS", "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de DOM XSS na página do catalogo", "DOM XSS" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 4,
                column: "Texto",
                value: "Procure por campos que reflitam sua entrada na interface.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 5,
                column: "Texto",
                value: "Tente pesquisar por produtos que não existem e observe a resposta.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Categoria", "Descricao", "Nome" },
                values: new object[] { "ExcessiveDataExposure", "Enumere alguns e-mails de usuários existentes.", "Encontrar usuários válidos" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 4,
                column: "Texto",
                value: "O desafio pode ser resolvido na página de detalhes do produto.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 5,
                column: "Texto",
                value: "Observe o retorno da chamada que lista comentários do produto.");
        }
    }
}
