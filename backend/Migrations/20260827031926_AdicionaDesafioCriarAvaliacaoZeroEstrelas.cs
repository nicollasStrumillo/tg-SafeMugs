using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDesafioCriarAvaliacaoZeroEstrelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 20,
                column: "DescricaoDetalhes",
                value: "Ao criar uma avaliação, o cliente envia no corpo da requisição o identificador do autor (usuarioId) junto com nota e comentario para POST |/api/produtos/avaliacoes/{produtoId}|. O servidor apenas verifica se o usuário informado existe e o vincula à avaliação, sem conferir se corresponde ao usuário autenticado. Interceptando a requisição disparada pelo diálogo de avaliação e trocando usuarioId pelo de outro usuário, é possível registrar uma avaliação em nome de outra pessoa.");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 21, "ImproperInputValidation", "Envie uma avaliação com nota zero.", "A interface de avaliação permite apenas notas de 0,5 a 5, mas o servidor não valida o intervalo. Interceptando POST |/api/produtos/avaliacoes/{produtoId}| e enviando nota com valor 0, a avaliação é persistida com zero estrelas.", 2, "Avaliação insatisfeita", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[] { 44, 21, 1, "A validação da nota acontece só no navegador." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 20,
                column: "DescricaoDetalhes",
                value: "Ao criar uma avaliação, o cliente envia no corpo da requisição o identificador do autor (usuarioId) junto com nota e comentario para POST /api/produtos/avaliacoes/{produtoId}. O servidor apenas verifica se o usuário informado existe e o vincula à avaliação, sem conferir se corresponde ao usuário autenticado. Interceptando a requisição disparada pelo diálogo de avaliação e trocando usuarioId pelo de outro usuário, é possível registrar uma avaliação em nome de outra pessoa.");
        }
    }
}
