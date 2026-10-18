using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioExtrairHashSenha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 26, "SqlInjection", "Obtenha a hash de senha de um usuário através de injeção SQL.", "A consulta que pesquisa pedidos por número concatena o termo digitado diretamente na string SQL, permitindo injeção. Como ela retorna seis colunas, um |UNION SELECT| precisa trazer o mesmo número de colunas para ser aceito. A coluna do número do pedido é textual, então consegue exibir strings vindas de outras tabelas — como a coluna |HashSenha| (string) da tabela |usuarios|. Combinar colunas com |CONCAT| permite trazer mais de um dado na mesma coluna textual, e finalizar com |%23| comenta o restante da consulta original.", 4, "Capture o hash da senha de um usuário", false, "/pedidos" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 53, 26, 1, "A pesquisa de pedidos permite anexar um UNION SELECT ao termo pesquisado." },
                    { 54, 26, 2, "A coluna HashSenha, do tipo string na tabela usuarios, cabe na coluna textual do número do pedido." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 26);
        }
    }
}
