using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDesafioReflectedXss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido" },
                values: new object[] { 18, "ReflectedXSS", "Faça a API refletir, no corpo de uma resposta, o payload |<iframe src=\"javascript:alert(`XSS`)\">| de uma forma que ele seja executado no navegador do cliente.", "Ao buscar um produto pelo nome, a API recebe o termo pesquisado como parâmetro da requisição. Quando o produto não é encontrado, a resposta de erro devolve esse mesmo termo de volta, sem tratamento, dentro da mensagem. No cliente, esse conteúdo refletido é inserido diretamente na página sem sanitização, de modo que qualquer HTML presente no termo é interpretado pelo navegador. O payload |?nome=<iframe src=\"javascript:alert(`XSS`)\">|, por exemplo, faz o servidor devolver o |<iframe>| na resposta, que então é renderizado e executado. Por se tratar de um conteúdo que parte do servidor e volta no corpo da resposta, trata-se de um Reflected XSS — diferente do DOM XSS, em que o payload nunca sai do navegador.", 3, "Reflected XSS", false });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[] { 40, 18, 1, "Procure uma funcionalidade de busca que devolva, na resposta, algo que você enviou na requisição." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
