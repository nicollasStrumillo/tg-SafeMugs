using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaNovoDesafioVisualizarCarrinhoAlheio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 13, "IDOR", "Visualize o carrinho de compras de outro usuário.", "A rota que busca o carrinho de compras recebe o identificador do usuário diretamente pela URL e o utiliza para localizar o carrinho, sem verificar se ele corresponde ao usuário autenticado. Ao interceptar a requisição que a tela de carrinho dispara e trocar o identificador pelo de outro cliente, o servidor devolve o carrinho completo do alvo: itens, quantidades e preços — acompanhados dos dados pessoais do dono, como nome, e-mail, telefone e endereço.\nO servidor confia que o parâmetro recebido na URL identifica o próprio dono do recurso, quando deveria validar que ele corresponde ao usuário autenticado antes de retornar qualquer dado, caracterizando uma Insecure Direct Object Reference (IDOR).", 2, "Visualizar carrinho de outro usuário", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 29, 13, 1, "A rota que busca o carrinho recebe o identificador do usuário diretamente pela URL." },
                    { 30, 13, 2, "Troque o identificador da requisição pelo de outro usuário e observe a resposta." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
