using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaSeedDataParaNovosProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "produtos",
                columns: new[] { "Id", "Ativo", "CategoriaProdutoId", "Descricao", "DtAtualizacao", "DtCadastro", "Estoque", "Nome", "Preco", "UrlImagemProduto" },
                values: new object[,]
                {
                    { 8, true, 3, "Caneca de ceramica artesanal em tons azuis e terracota, com selo em relevo Purrista Cat Cafe.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 8, "Caneca Purrista Cat Cafe", 69.90m, "/imagens/produto/mug_cat_cafe.jpg" },
                    { 9, true, 1, "Caneca branca com frase divertida CSS is Amazing, ideal para programadores e devs.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 22, "Caneca CSS is Amazing", 52.90m, "/imagens/produto/mug_css.jpg" },
                    { 10, true, 2, "Caneca branca ilustrada com diversas raças de gatos e seus nomes em japones e ingles.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 14, "Caneca Desenhos de Gatos", 61.90m, "/imagens/produto/mug_desenhos_gatos.jpg" },
                    { 11, true, 1, "Caneca clara com ilustracao de cafeteira italiana e xicara fumegante, tema cafe.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 17, "Caneca Icones Cafe", 55.90m, "/imagens/produto/mug_icones_cafe.jpg" },
                    { 12, true, 1, "Caneca preta fosca de design minimalista, combina com qualquer ambiente.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 30, "Caneca Preta Fosca", 47.90m, "/imagens/produto/mug_preta.jpg" },
                    { 13, true, 1, "Caneca rosa clara de acabamento fosco, visual delicado e moderno.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 19, "Caneca Rosa", 53.90m, "/imagens/produto/mug_rosa.jpg" },
                    { 14, true, 3, "Caneca de ceramica artesanal com rosto sonolento gravado, acabamento rustico.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 11, "Caneca Sono", 58.90m, "/imagens/produto/mug_sono.jpg" },
                    { 15, true, 1, "Caneca branca com rosto sorridente, para o dia a dia com bom humor.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 16, "Caneca Sorriso Engraçado", 49.90m, "/imagens/produto/mug_sorriso.jpg" },
                    { 16, true, 1, "Caneca branca com a frase World's Best Boss, otima opção de presente para chefes.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 13, "Caneca World's Best Boss", 54.90m, "/imagens/produto/mug_worlds_best_boss.jpg" }
                });

            migrationBuilder.InsertData(
                table: "avaliacoes",
                columns: new[] { "Id", "Comentario", "DtAtualizacao", "DtCadastro", "Nota", "ProdutoId", "UsuarioId" },
                values: new object[,]
                {
                    { 16, "Acabamento artesanal lindo e ceramica muito bem feita.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 8, 2 },
                    { 17, "Detalhe em relevo da um charme especial, adorei o tom terracota.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 8, 4 },
                    { 18, "Caneca charmosa e diferente, combina bem com decoracao.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4f, 8, 7 },
                    { 19, "Frase divertida e estampa muito nitida, perfeita para o trabalho.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 9, 1 },
                    { 20, "Otima para presentear colegas da area de tecnologia.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 9, 3 },
                    { 21, "Material de boa qualidade e impressao bem feita.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4f, 9, 5 },
                    { 22, "Chegou bem embalada e o acabamento e excelente.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 9, 6 },
                    { 23, "Ilustracoes muito fofas e detalhadas, adorei a proposta.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 10, 2 },
                    { 24, "Estampa criativa e cores muito bonitas.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 10, 7 },
                    { 25, "Design elegante e tema de cafe muito bem representado.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 11, 1 },
                    { 26, "Visual delicado e acabamento caprichado.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4f, 11, 4 },
                    { 27, "Perfeita para a rotina do cafe da manha.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 11, 6 },
                    { 28, "Acabamento fosco muito bonito e sofisticado.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 12, 3 },
                    { 29, "Minimalista e versatil, combina com tudo.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 12, 5 },
                    { 30, "Tom de rosa suave e acabamento impecavel.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 13, 2 },
                    { 31, "Caneca delicada e com tamanho otimo para o dia a dia.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4f, 13, 4 },
                    { 32, "Cor linda e material de otima qualidade.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 13, 1 },
                    { 33, "Visual moderno e muito agradavel.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 13, 7 },
                    { 34, "Rostinho sonolento muito fofo e acabamento rustico lindo.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 14, 3 },
                    { 35, "Peca diferente e com personalidade, gostei bastante.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4f, 14, 5 },
                    { 36, "Ceramica bem trabalhada e detalhe gravado caprichado.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 14, 6 },
                    { 37, "Rosto sorridente traz bom humor para o dia.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 15, 1 },
                    { 38, "Caneca leve e com estampa muito simpatica.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 15, 4 },
                    { 39, "Frase divertida e otima ideia para presente.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4.5f, 16, 2 },
                    { 40, "Estampa bem feita e caneca de boa qualidade.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5f, 16, 3 },
                    { 41, "Gostei do acabamento e da proposta descontraida.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4f, 16, 7 }
                });

            migrationBuilder.InsertData(
                table: "comentarios_produto",
                columns: new[] { "Id", "Comentario", "ProdutoId", "UsuarioId" },
                values: new object[,]
                {
                    { 19, "Adorei o tom azul e o detalhe em relevo.", 8, 2 },
                    { 20, "Chegou bem embalada.", 8, null },
                    { 21, "Acabamento artesanal muito bonito.", 8, 5 },
                    { 22, "Frase muito divertida.", 9, 1 },
                    { 23, "Presente perfeito para devs.", 9, null },
                    { 24, "Ilustrações muito fofas.", 10, 7 },
                    { 25, "Os desenhos ficaram lindos.", 10, null },
                    { 26, "Minha coleção favorita.", 10, 2 },
                    { 27, "Cores bem vivas.", 10, 4 },
                    { 28, "Tema café ficou ótimo.", 11, 3 },
                    { 29, "Muito elegante.", 11, null },
                    { 30, "Uso todo dia no trabalho.", 11, 6 },
                    { 31, "Preta fosca muito chique.", 12, 1 },
                    { 32, "Combina com qualquer ambiente.", 12, null },
                    { 33, "Tonalidade rosa bem delicada.", 13, 4 },
                    { 34, "Linda ao vivo.", 13, null },
                    { 35, "Tamanho ideal.", 13, 2 },
                    { 36, "Rostinho muito simpático.", 14, 6 },
                    { 37, "Acabamento rústico lindo.", 14, null },
                    { 38, "Sorriso deixa o dia mais leve.", 15, 3 },
                    { 39, "Bem divertida.", 15, null },
                    { 40, "Ótima para o café da tarde.", 15, 5 },
                    { 41, "Estampa bem nítida.", 15, 1 },
                    { 42, "Presenteei meu chefe e ele adorou.", 16, 7 },
                    { 43, "Frase muito criativa.", 16, null }
                });

            migrationBuilder.InsertData(
                table: "itens_carrinho",
                columns: new[] { "Id", "CarrinhoId", "PrecoUnitario", "ProdutoId", "Quantidade" },
                values: new object[,]
                {
                    { 9, 3, 69.90m, 8, 1 },
                    { 10, 6, 52.90m, 9, 2 },
                    { 11, 7, 61.90m, 10, 1 },
                    { 12, 3, 55.90m, 11, 1 },
                    { 13, 8, 47.90m, 12, 1 },
                    { 14, 2, 53.90m, 13, 1 },
                    { 15, 1, 58.90m, 14, 1 },
                    { 16, 5, 49.90m, 15, 2 },
                    { 17, 6, 54.90m, 16, 1 },
                    { 18, 7, 69.90m, 8, 1 },
                    { 19, 3, 47.90m, 12, 1 },
                    { 20, 8, 52.90m, 9, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "comentarios_produto",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "itens_carrinho",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
