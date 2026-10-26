using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioDOS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 29, "XmlExternalEntity", "Faça uma requisição que deixe o servidor ocupado por um tempo.", "Parsers que processam XML não confiável sem limites adequados podem consumir recursos desproporcionais ao conteúdo recebido. Em especial, a expansão de entidades em DTDs pode transformar uma entrada pequena em um grande volume de texto, consumindo CPU e memória e reduzindo a capacidade do servidor de atender outras requisições. Rejeitar DTDs ou limitar rigorosamente a expansão reduz esse risco.", 4, "DOS", false, "/administracao" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 56, 29, 1, "A página de Administração oferece a reposição de estoque por XML; explore esse fluxo." },
                    { 57, 29, 2, "O tamanho enviado não é o único custo; considere o trabalho que o parser faz ao interpretar o XML." },
                    { 58, 29, 3, "Entidades declaradas em um DTD podem referenciar outras entidades, fazendo o conteúdo processado crescer rapidamente." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 29);
        }
    }
}
