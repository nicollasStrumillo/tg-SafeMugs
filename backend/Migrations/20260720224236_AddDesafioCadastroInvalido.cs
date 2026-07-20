using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioCadastroInvalido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Descricao", "Nome" },
                values: new object[] { "Tente burlar a validação do formulário de cadastro.", "Cadastro inválido" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 9,
                column: "Texto",
                value: "A validação dos campos pode ser insuficiente no lado do servidor.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Descricao", "Nome" },
                values: new object[] { "Teste a validação do formulário de cadastro.", "Validar cadastro" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 9,
                column: "Texto",
                value: "A validação dos campos pode ser insuficiente.");
        }
    }
}
