using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioCarrinhoQuantidadeNegativa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 25, "ParameterTampering", "Faça um carrinho ter uma quantidade inválida de produtos.", "A alteração de quantidade no carrinho confia inteiramente no valor enviado pelo cliente. A interface da loja envia apenas incrementos de ±1, mas o servidor não verifica se a quantidade recebida é positiva antes de gravar: ao interceptar a requisição de adicionar um produto e alterar o campo |\"quantidade\": -5| no corpo enviado, o item é salvo com quantidade negativa. Como o total do carrinho é calculado multiplicando o preço unitário pela quantidade, o carrinho passa a exibir um valor negativo — como se a loja devesse dinheiro ao cliente. Valores de negócio nunca devem ser aceitos sem validação no servidor, pois o cliente tem controle total sobre o que envia.", 2, "Quantidade inválida no carrinho", false, "/carrinho" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[] { 52, 25, 1, "Intercepte a requisição de adicionar um produto ao carrinho e analise os parâmetros que ela carrega." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 25);
        }
    }
}
