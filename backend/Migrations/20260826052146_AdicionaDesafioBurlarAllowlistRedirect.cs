using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDesafioBurlarAllowlistRedirect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 15, "OpenRedirect", "Faça a API redirecionar para um domínio que não seja o GitHub, burlando a allowlist de redirect.", "O botão do GitHub na navegação lateral não abre o link diretamente: ele chama o endpoint |/api/redirect?url=...|, que deve garantir que o destino seja um host confiável antes de redirecionar. Para isso, o backend mantém uma allowlist com o host permitido e valida a URL recebida checando se ela contém o host como substring — ou seja, basta que a string |https://github.com| apareça em qualquer posição da URL para que ela seja considerada segura. Como essa checagem ignora a estrutura real da URL, é possível embutir o host permitido em posições que não correspondem ao destino efetivo. Em uma URL, o trecho antes do |@| é tratado como informação de usuário e o host vem depois dele: o payload |https://github.com@evil.com| passa pela allowlist (contém |https://github.com|), mas o navegador direciona ao host |evil.com|. Outro bypass análogo usa a query string — |https://evil.com?https://github.com| — em que o host permitido aparece após o |?|, como parte dos parâmetros, sem identificar o host real. Um redirect confiável deve analisar a URL como uma estrutura (parseando host, esquema e caminho) e comparar o host efetivo contra a allowlist, nunca realizar um simples casamento de substring.", 2, "Burlar a allowlist de redirect", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 33, 15, 1, "A allowlist confere apenas se a URL contém o host permitido como substring, sem examinar o destino real." },
                    { 34, 15, 2, "Pense em como embutir o host permitido dentro de uma URL que aponta para outro domínio." },
                    { 35, 15, 3, "Em uma URL, o segmento antes do @ é tratado como informação de usuário e o host vem depois." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
