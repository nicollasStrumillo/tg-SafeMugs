using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioXxeReposicaoEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 28, "XmlExternalEntity", "Leia o arquivo local DesafioAssets/xxe.txt explorando o processamento de XML.", "Uma vulnerabilidade XXE ocorre quando um parser aceita DTDs e resolve entidades externas definidas em um XML não confiável. Como o parser atua no servidor, essas referências podem levar a aplicação a acessar arquivos locais ou recursos externos e incorporar seus conteúdos ao documento, expondo informações que não deveriam estar disponíveis. XML recebido de usuários deve ser processado sem DTDs ou resolução externa, salvo quando esses recursos forem necessários e estiverem restritos.", 4, "Leia um arquivo local usando XXE", false, "/administracao" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[] { 55, 28, 1, "Um DTD pode declarar entidades externas que apontam para recursos locais; o caminho do arquivo está na descrição." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 28);
        }
    }
}
