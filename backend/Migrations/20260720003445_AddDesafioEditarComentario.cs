using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioEditarComentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "Edite um comentário que não pertence ao seu usuário.", 3, "Altere o comentário de outro usuário" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 17,
                column: "Texto",
                value: "Observe o corpo da requisição de editar comentário.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 18,
                column: "Texto",
                value: "Intercepte a requisição de editar comentário para mudar o seu payload.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Descricao", "Dificuldade", "Nome" },
                values: new object[] { "Modifique os parâmetros de filtro e ordenação da listagem para encontrar informações sensíveis.", 2, "Alterar ordenação do catálogo" });

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 17,
                column: "Texto",
                value: "Filtros e ordenação costumam vir por query string.");

            migrationBuilder.UpdateData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 18,
                column: "Texto",
                value: "O backend pode estar ordenando por qualquer parâmetro que ele receber.");
        }
    }
}
