using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDesafioPathTraversalFotoPerfil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 17, "PathTraversal", "Sobrescreva a imagem de um dos produtos exibidos no catálogo da loja.", "O catálogo exibe imagens guardadas na pasta |/imagens/produto|, cada uma referenciada por um produto do banco. Um endpoint que recebe o upload de um arquivo monta o caminho de destino concatenando o nome do arquivo informado pelo cliente diretamente em Path.Combine, sem sanitizar separadores nem segmentos |..|. Como |..| permite navegar para cima na árvore de diretórios, basta interceptar o upload e trocar o metadado filename por |../../produto/mug_behappy.jpg| (ou outra imagem real da pasta) para que o arquivo enviado sobrescreva a imagem daquele produto.", 4, "Alterar a imagem de um produto", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 38, 17, 1, "Procure uma funcionalidade do sistema que te permite fazer uploads de imagens." },
                    { 39, 17, 2, "Na tela de perfil, o upload de foto de perfil monta o caminho de destino a partir do nome do arquivo enviado pelo cliente, sem sanitizar." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 17);
        }
    }
}
