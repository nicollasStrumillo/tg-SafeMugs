using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaComentariosProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comentarios_produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Comentario = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    ProdutoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comentarios_produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comentarios_produto_produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comentarios_produto_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "comentarios_produto",
                columns: new[] { "Id", "Comentario", "ProdutoId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "Muito bonita pessoalmente.", 1, 1 },
                    { 2, "Chegou antes do prazo.", 1, null },
                    { 3, "A estampa ficou exatamente como nas fotos.", 1, 3 },
                    { 4, "Gostei bastante do acabamento.", 2, 2 },
                    { 5, "A textura é bem diferente.", 2, null },
                    { 6, "Combina muito com decoração em madeira.", 2, 5 },
                    { 7, "Minha filha adorou.", 3, 6 },
                    { 8, "A arte ficou muito bonita.", 3, null },
                    { 9, "Veio muito bem embalada.", 3, 1 },
                    { 10, "Comprei para dar de presente.", 4, 4 },
                    { 11, "A cor é ainda mais bonita ao vivo.", 4, null },
                    { 12, "Gostei do tamanho da caneca.", 4, 2 },
                    { 13, "Perfeita para quem gosta de cachorros.", 5, 5 },
                    { 14, "Entrega rápida.", 5, null },
                    { 15, "A impressão ficou muito nítida.", 5, 7 },
                    { 16, "Bem resistente.", 6, 3 },
                    { 17, "Ótima para café pela manhã.", 6, null },
                    { 18, "Produto conforme anunciado.", 6, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_comentarios_produto_ProdutoId",
                table: "comentarios_produto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_comentarios_produto_UsuarioId",
                table: "comentarios_produto",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comentarios_produto");
        }
    }
}
