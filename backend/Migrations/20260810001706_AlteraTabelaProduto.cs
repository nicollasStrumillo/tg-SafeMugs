using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteraTabelaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "imagens_produto");

            migrationBuilder.AddColumn<string>(
                name: "UrlImagemProduto",
                table: "produtos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlImagemProduto",
                value: "/imagens/produto/mug_behappy.jpg");

            migrationBuilder.UpdateData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlImagemProduto",
                value: "/imagens/produto/mug_ceramica_rustica.jpg");

            migrationBuilder.UpdateData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlImagemProduto",
                value: "/imagens/produto/mug_coala.jpg");

            migrationBuilder.UpdateData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlImagemProduto",
                value: "/imagens/produto/mug_coracao.jpg");

            migrationBuilder.UpdateData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlImagemProduto",
                value: "/imagens/produto/mug_dogpan.jpg");

            migrationBuilder.UpdateData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 6,
                column: "UrlImagemProduto",
                value: "/imagens/produto/mug_vermelha_cafe.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImagemProduto",
                table: "produtos");

            migrationBuilder.CreateTable(
                name: "imagens_produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    Legenda = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlImagem = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagens_produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_imagens_produto_produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "imagens_produto",
                columns: new[] { "Id", "Legenda", "ProdutoId", "UrlImagem" },
                values: new object[,]
                {
                    { 1, "Caneca Be Happy", 1, "/imagens/produto/mug_behappy.jpg" },
                    { 2, "Caneca Ceramica Rustica", 2, "/imagens/produto/mug_ceramica_rustica.jpg" },
                    { 3, "Caneca Coala", 3, "/imagens/produto/mug_coala.jpg" },
                    { 4, "Caneca Coracao", 4, "/imagens/produto/mug_coracao.jpg" },
                    { 5, "Caneca cachorro", 5, "/imagens/produto/mug_dogpan.jpg" },
                    { 6, "Caneca Vermelha Cafe", 6, "/imagens/produto/mug_vermelha_cafe.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_imagens_produto_ProdutoId",
                table: "imagens_produto",
                column: "ProdutoId");
        }
    }
}
