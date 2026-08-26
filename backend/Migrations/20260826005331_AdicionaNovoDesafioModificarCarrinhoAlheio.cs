using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaNovoDesafioModificarCarrinhoAlheio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nome",
                value: "Visualizar o carrinho de outro usuário");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 14, "IDOR", "Modifique o carrinho de compras de outro usuário.", "As requisições que alteram o carrinho — adicionar e remover produtos — carregam o identificador do dono no corpo da requisição, e o servidor o utiliza diretamente para localizar o carrinho a ser modificado, sem verificar se ele corresponde ao usuário autenticado. Ao interceptar a requisição que a tela de carrinho dispara e trocar o identificador pelo de outro cliente, é possível adicionar produtos ao carrinho alheio ou remover itens que ele continha — inclusive esvaziá-lo por completo.\nO servidor confia que o identificador recebido no corpo da requisição identifica o próprio dono do recurso, quando deveria validar que ele corresponde ao usuário autenticado antes de alterar qualquer dado, caracterizando uma Insecure Direct Object Reference (IDOR).", 2, "Modificar o carrinho de outro usuário", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 31, 14, 1, "As requisições que adicionam e removem produtos do carrinho carregam o identificador do usuário no corpo." },
                    { 32, 14, 2, "Troque o identificador pelo de outro usuário e observe o carrinho dele mudar." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nome",
                value: "Visualizar carrinho de outro usuário");
        }
    }
}
