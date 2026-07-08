using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class DesafiosAlteraModeloESeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "progressos_desafio");

            migrationBuilder.AddColumn<bool>(
                name: "Resolvido",
                table: "desafios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 11,
                column: "Resolvido",
                value: false);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 12,
                column: "Resolvido",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolvido",
                table: "desafios");

            migrationBuilder.CreateTable(
                name: "progressos_desafio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DesafioId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DtAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DtResolvido = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    QtDicasDesbloqueadas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progressos_desafio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_progressos_desafio_desafios_DesafioId",
                        column: x => x.DesafioId,
                        principalTable: "desafios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_progressos_desafio_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_progressos_desafio_DesafioId",
                table: "progressos_desafio",
                column: "DesafioId");

            migrationBuilder.CreateIndex(
                name: "IX_progressos_desafio_UsuarioId_DesafioId",
                table: "progressos_desafio",
                columns: new[] { "UsuarioId", "DesafioId" },
                unique: true);
        }
    }
}
