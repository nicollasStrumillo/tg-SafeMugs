using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteramModeloUsuarioESeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UrlImagemPerfil",
                table: "usuarios",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_profile.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_profile.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_profile.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_profile.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_profile.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_profile.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_profile.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 8,
                column: "UrlImagemPerfil",
                value: "/imagens/perfil/generic_admin_profile.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UrlImagemPerfil",
                table: "usuarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlImagemPerfil",
                value: "");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlImagemPerfil",
                value: "");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlImagemPerfil",
                value: "");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlImagemPerfil",
                value: "");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlImagemPerfil",
                value: "");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6,
                column: "UrlImagemPerfil",
                value: "");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7,
                column: "UrlImagemPerfil",
                value: "");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 8,
                column: "UrlImagemPerfil",
                value: "");
        }
    }
}
