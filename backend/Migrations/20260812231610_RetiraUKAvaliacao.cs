using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RetiraUKAvaliacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_avaliacoes_usuarios_UsuarioId",
                table: "avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_avaliacoes_UsuarioId_ProdutoId",
                table: "avaliacoes");

            migrationBuilder.CreateIndex(
                name: "IX_avaliacoes_UsuarioId",
                table: "avaliacoes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_avaliacoes_usuarios_UsuarioId",
                table: "avaliacoes",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_avaliacoes_UsuarioId",
                table: "avaliacoes");

            migrationBuilder.CreateIndex(
                name: "IX_avaliacoes_UsuarioId_ProdutoId",
                table: "avaliacoes",
                columns: new[] { "UsuarioId", "ProdutoId" },
                unique: true);
        }
    }
}
