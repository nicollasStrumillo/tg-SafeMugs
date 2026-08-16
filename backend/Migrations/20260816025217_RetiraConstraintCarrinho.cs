using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RetiraConstraintCarrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_carrinhos_usuarios_UsuarioId",
                table: "carrinhos");

            migrationBuilder.DropIndex(
                name: "IX_carrinhos_UsuarioId",
                table: "carrinhos");

            migrationBuilder.CreateIndex(
                name: "IX_carrinhos_UsuarioId",
                table: "carrinhos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_carrinhos_usuarios_UsuarioId",
                table: "carrinhos",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_carrinhos_usuarios_UsuarioId",
                table: "carrinhos");

            migrationBuilder.DropIndex(
                name: "IX_carrinhos_UsuarioId",
                table: "carrinhos");

            migrationBuilder.CreateIndex(
                name: "IX_carrinhos_UsuarioId",
                table: "carrinhos",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_carrinhos_usuarios_UsuarioId",
                table: "carrinhos",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
