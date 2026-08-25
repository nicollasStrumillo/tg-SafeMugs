using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AumentaDescricaoDetalhesDesafioEAdicionaDesafioListarTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescricaoDetalhes",
                table: "desafios",
                type: "varchar(1500)",
                maxLength: 1500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(800)",
                oldMaxLength: 800)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 11, "SqlInjection", "Obtenha o nome de todas as tabelas do banco de dados do SafeMugs.", "A consulta SQL que pesquisa pedidos por número concatena o termo digitado diretamente na string da consulta, permitindo injeção de SQL. Como a consulta original retorna seis colunas, um |UNION SELECT| anexado ao payload só é aceito se trouxer exatamente o mesmo número de colunas, e as mensagens de erro da API expõem quando essa contagem está errada, funcionando como um oráculo para descobri-la. O MySQL mantém os metadados do banco na view |information_schema.tables|: selecionar a coluna |TABLE_NAME| com o filtro |WHERE TABLE_SCHEMA = DATABASE()| retorna o nome de todas as tabelas do esquema atual. O payload |' UNION SELECT 1,TABLE_NAME,3,4,CURDATE(),6 FROM information_schema.tables WHERE TABLE_SCHEMA = DATABASE()%23|, por exemplo, lista todas as tabelas na coluna do número do pedido, enquanto o |%23| ('#' encodado para URL) transforma o restante da linha em comentário, neutralizando o filtro que restringiria os resultados ao usuário autenticado.", 4, "Enumere as tabelas do banco", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 22, 11, 1, "A consulta que pesquisa pedidos por número concatena o termo pesquisado diretamente na string SQL." },
                    { 23, 11, 2, "Um UNION SELECT só é aceito com o mesmo número de colunas da consulta original. As mensagens de erro da API indicam quando essa contagem está errada." },
                    { 24, 11, 3, "Os nomes das tabelas ficam na coluna TABLE_NAME da view information_schema.tables. Finalize o payload com # (ou -- seguido de espaço) para comentar o restante da consulta." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoDetalhes",
                table: "desafios",
                type: "varchar(800)",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1500)",
                oldMaxLength: 1500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
