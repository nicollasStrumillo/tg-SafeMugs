using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaNovoDesafioProdutoInvalidoESeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "produtos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true);

            migrationBuilder.InsertData(
                table: "carrinhos",
                columns: new[] { "Id", "DtAtualizacao", "DtCadastro", "Status", "UsuarioId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 1 },
                    { 2, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 2 },
                    { 3, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 3 },
                    { 4, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 4 },
                    { 5, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 5 },
                    { 6, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 6 },
                    { 7, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 7 },
                    { 8, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Ativo", 8 }
                });

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 12, "ImproperInputValidation", "Adicione a um carrinho um produto que não devia estar disponível.", "Nem todos os produtos registrados no banco do SafeMugs aparecem no catálogo: existe um item com estoque zerado e marcado como inativo, invisível para qualquer cliente da loja. O fluxo que adiciona um produto ao carrinho recebe o identificador do produto no corpo da requisição e apenas verifica se ele existe no banco — não confere se o produto está ativo nem se há estoque disponível. Para explorá-lo, primeiro é preciso descobrir o produto oculto: consultando o carrinho de outro usuário pela rota |/api/carrinho/{usuarioId}|, que expõe todos os itens e seus produtos, ou extraindo os registros da tabela de produtos com a injeção de SQL da pesquisa de pedidos. Com o identificador em mãos, basta interceptar a requisição |PATCH /api/carrinho/adicionar| e substituir o |produtoId| pelo do produto indisponível: o servidor aceita o item e o adiciona ao carrinho sem qualquer questionamento. O backend confia cegamente que o cliente só referencia produtos que podem ser vendidos, quando deveria validar o estado do recurso — ativo e com estoque — antes de aceitá-lo em qualquer operação, caracterizando uma validação de entrada imprópria no servidor.", 3, "Produto inválido", false });

            migrationBuilder.InsertData(
                table: "produtos",
                columns: new[] { "Id", "CategoriaProdutoId", "Descricao", "DtAtualizacao", "DtCadastro", "Estoque", "Nome", "Preco", "UrlImagemProduto" },
                values: new object[] { 7, 2, "Caneca branca com ilustracao de um gato cinza segurando um morango.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 0, "Caneca Gato Cinza", 56.90m, "/imagens/produto/mug_gato_morango.jpg" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 25, 12, 1, "Nem todos os produtos registrados no banco aparecem no catálogo da loja." },
                    { 26, 12, 2, "Existe um endpoint que revela indevidamente os produtos que participaram de outras operações do sistema" },
                    { 27, 12, 3, "Os carrinhos de outros usuários podem revelar produtos que não estão à venda." },
                    { 28, 12, 4, "Intercepte a requisição que adiciona um produto ao carrinho e troque o produtoId pelo do produto indisponível." }
                });

            migrationBuilder.InsertData(
                table: "itens_carrinho",
                columns: new[] { "Id", "CarrinhoId", "PrecoUnitario", "ProdutoId", "Quantidade" },
                values: new object[,]
                {
                    { 1, 1, 64.90m, 3, 1 },
                    { 2, 1, 49.90m, 6, 2 },
                    { 3, 2, 54.90m, 2, 1 },
                    { 4, 2, 58.90m, 5, 1 },
                    { 5, 4, 59.90m, 1, 1 },
                    { 6, 4, 62.90m, 4, 1 },
                    { 7, 4, 56.90m, 7, 2 },
                    { 8, 5, 64.90m, 3, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "carrinhos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "produtos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);
        }
    }
}
