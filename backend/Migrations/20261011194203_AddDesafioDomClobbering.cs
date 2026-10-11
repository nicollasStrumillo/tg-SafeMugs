using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioDomClobbering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 24, "DomXSS", "Envie o cookie |safemugs.token| de um usuário para o endpoint GET |/api/desafios/xss-callback|, explorando DOM Clobbering no link do rodapé da aplicação.", "O botão do rodapé não usa a URL fixa do código: ele procura no documento inteiro o elemento com id githubLink e navega para o href encontrado. Como a página de avaliações renderiza o e-mail dos autores como HTML e o rodapé vem depois do conteúdo no documento, um elemento com o mesmo id plantado no e-mail é encontrado primeiro e assume o lugar do link legítimo. A partir daí, o href lido é o do atacante — e uma URL javascript: navega executando código no contexto da página da vítima. De forma geral, consultas globais por id em documentos que também exibem HTML de usuários são frágeis: prefira o próprio alvo do evento ou referências com escopo, e nunca navegue para esquemas não validados.", 5, "DOM Clobbering", false, "/avaliacoes-produto" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 50, 24, 1, "O link 'Abrir o GitHub' do rodapé é localizado pelo id githubLink: um elemento com esse id renderizado antes dele na página assume o seu lugar." },
                    { 51, 24, 2, "O e-mail exibido na tela de avaliações do produto é renderizado como HTML — ele pode carregar o elemento que sobrepõe o link." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 24);
        }
    }
}
