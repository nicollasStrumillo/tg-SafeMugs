using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteramModeloUsuarioESeedDataImagens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_enderecos_usuarios_UsuarioId",
                table: "enderecos");

            migrationBuilder.DropIndex(
                name: "IX_enderecos_UsuarioId",
                table: "enderecos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "enderecos");

            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImagemPerfil",
                table: "usuarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlImagem",
                value: "/imagens/produto/mug_behappy.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlImagem",
                value: "/imagens/produto/mug_ceramica_rustica.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlImagem",
                value: "/imagens/produto/mug_coala.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlImagem",
                value: "/imagens/produto/mug_coracao.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlImagem",
                value: "/imagens/produto/mug_dogpan.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 6,
                column: "UrlImagem",
                value: "/imagens/produto/mug_vermelha_cafe.jpg");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EnderecoId", "UrlImagemPerfil" },
                values: new object[] { null, "" });

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_EnderecoId",
                table: "usuarios",
                column: "EnderecoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_enderecos_EnderecoId",
                table: "usuarios",
                column: "EnderecoId",
                principalTable: "enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_enderecos_EnderecoId",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_EnderecoId",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "UrlImagemPerfil",
                table: "usuarios");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "enderecos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlImagem",
                value: "/imagens/mug_behappy.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlImagem",
                value: "/imagens/mug_ceramica_rustica.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlImagem",
                value: "/imagens/mug_coala.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlImagem",
                value: "/imagens/mug_coracao.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlImagem",
                value: "/imagens/mug_dogpan.jpg");

            migrationBuilder.UpdateData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 6,
                column: "UrlImagem",
                value: "/imagens/mug_vermelha_cafe.jpg");

            migrationBuilder.CreateIndex(
                name: "IX_enderecos_UsuarioId",
                table: "enderecos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_enderecos_usuarios_UsuarioId",
                table: "enderecos",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
