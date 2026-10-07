using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioSsrfServicoOculto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 22, "ServerSideRequestForgery", "Descubra o serviço escondido em uma porta interna do servidor e recupere o segredo que ele guarda.", "O campo de URL da foto de perfil faz o próprio backend buscar o endereço informado — inclusive endereços que apontam para o próprio servidor. Isso permite usá-lo como sonda para enumerar portas internas: uma porta fechada devolve um erro controlado de conexão, enquanto uma porta em que algum serviço responde devolve uma resposta HTTP; quando essa resposta não é uma imagem, o corpo é refletido na mensagem de erro da requisição. Varrendo portas comuns de serviços web encontra-se um listener interno em |http://127.0.0.1:9090|, não publicado no host, cujo GET na raiz devolve em texto puro um segredo usado internamente pela aplicação. Como a porta não é exposta externamente, nenhum scan externo a encontra: a descoberta só é possível induzindo o próprio servidor a fazer a requisição — um Server-Side Request Forgery usado como varredura de portas internas.", 5, "Serviço interno oculto", false, "/perfil" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 45, 22, 1, "O campo de URL da foto de perfil faz o próprio servidor buscar o endereço informado — inclusive endereços que apontam para ele mesmo." },
                    { 46, 22, 2, "Compare a resposta da API para uma porta fechada com a resposta de uma porta em que algum serviço responde." },
                    { 47, 22, 3, "Serviços web internos costumam escutar em portas como 8080, 8443 e 9090. Faça um GET nelas pelo campo de URL." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 22);
        }
    }
}
