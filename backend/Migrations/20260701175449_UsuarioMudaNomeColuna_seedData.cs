using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioMudaNomeColuna_seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "usuarios",
                newName: "HashSenha");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "HashSenha",
                value: "1b13939cd7d77f68bac85931bfbb0a36");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "HashSenha",
                value: "1b13939cd7d77f68bac85931bfbb0a36");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "HashSenha",
                value: "1b13939cd7d77f68bac85931bfbb0a36");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "HashSenha",
                value: "1b13939cd7d77f68bac85931bfbb0a36");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "HashSenha",
                value: "1b13939cd7d77f68bac85931bfbb0a36");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6,
                column: "HashSenha",
                value: "1b13939cd7d77f68bac85931bfbb0a36");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7,
                column: "HashSenha",
                value: "1b13939cd7d77f68bac85931bfbb0a36");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashSenha",
                table: "usuarios",
                newName: "Senha");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Telefone",
                keyValue: null,
                column: "Telefone",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "Seed@12345");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Senha",
                value: "Seed@12345");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "Senha",
                value: "Seed@12345");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Senha",
                value: "Seed@12345");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Senha",
                value: "Seed@12345");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6,
                column: "Senha",
                value: "Seed@12345");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Senha",
                value: "Seed@12345");
        }
    }
}
