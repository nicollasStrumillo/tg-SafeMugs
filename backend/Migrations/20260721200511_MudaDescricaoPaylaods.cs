using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MudaDescricaoPaylaods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descricao",
                value: "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de DOM XSS na página do catalogo");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Descricao",
                value: "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de Stored XSS na página do catalogo");
        }
    }
}
