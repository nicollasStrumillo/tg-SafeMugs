using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDesafioServerSideRequestForger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 16, "ServerSideRequestForgery", "Alcance o endpoint interno |/api/usuario/requisicao-interna|, que só responde a requisições originadas pelo próprio servidor.", "Existe um endpoint interno em |/api/usuario/requisicao-interna| que valida um header secreto adicionado apenas quando a requisição é disparada pelo próprio backend — por isso não pode ser alcançado diretamente pelo navegador ou pelo Burp. O caminho é induzir o servidor a emitir uma requisição HTTP para uma URL controlada por você, apontando-a para o loopback. O payload esperado é |http://127.0.0.1:8080/api/usuario/requisicao-interna|: o servidor faz a requisição em seu nome, alcança o endpoint interno e o desafio é marcado.", 3, "Endpoint interno", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 36, 16, 1, "Procure um local da aplicação onde é possível induzir o servidor a fazer uma requisição a uma url escolhida por você." },
                    { 37, 16, 2, "Na tela de perfil, o campo de upload de foto por URL faz o próprio servidor baixar a imagem informada, sem validar o destino." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
