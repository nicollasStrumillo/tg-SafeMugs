using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteraModelDesafio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlMitigacao",
                table: "desafios");

            migrationBuilder.AddColumn<string>(
                name: "DescricaoDetalhes",
                table: "desafios",
                type: "varchar(800)",
                maxLength: 800,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 1,
                column: "DescricaoDetalhes",
                value: "A consulta SQL que busca o usuário no banco de dados não trata corretamente a entrada de parâmetros, permitindo que seja possível injetar código SQL malicioso. O payload |' OR p.Nome = \"Administrador\";-- |, por exemplo, utiliza a aspas simples para quebrar a string da consulta, o operador OR adiciona mais uma condição ao Select e \";-- \" finaliza a consulta e transforma o restante da linha em um comentário. Isso permite que você acesse a conta de um usuário com perfil administrativo sem precisar conhecer a senha.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "DescricaoDetalhes",
                value: "O payload |<iframe src=\"javascript:alert(`XSS`)\">| é um exemplo de ataque de DOM XSS. Ele insere um iframe que executa código JavaScript quando a página é carregada. Isso pode ser usado para roubar informações do usuário ou executar ações não autorizadas.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "DescricaoDetalhes",
                value: "O payload |<iframe src=\"javascript:alert(`XSS`)\">| é um exemplo de ataque de Stored XSS. Ele insere um iframe que executa código JavaScript quando a página é carregada. Isso pode ser usado para roubar informações do usuário ou executar ações não autorizadas.");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                column: "DescricaoDetalhes",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10,
                column: "DescricaoDetalhes",
                value: "placeholder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescricaoDetalhes",
                table: "desafios");

            migrationBuilder.AddColumn<string>(
                name: "UrlMitigacao",
                table: "desafios",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9,
                column: "UrlMitigacao",
                value: "url_placeholder");

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10,
                column: "UrlMitigacao",
                value: "url_placeholder");
        }
    }
}
