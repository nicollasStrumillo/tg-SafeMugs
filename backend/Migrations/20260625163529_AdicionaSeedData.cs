using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "categorias_produto",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Canecas com estampas afetivas e divertidas", "Canecas Decoradas" },
                    { 2, "Modelos inspirados em pets e personagens", "Canecas Tematicas" },
                    { 3, "Pecas com acabamento artesanal e visual mais natural", "Canecas Rusticas" }
                });

            migrationBuilder.InsertData(
                table: "perfis",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Perfil padrao para compras e avaliacoes", "Cliente" },
                    { 2, "Perfil para gestao interna da loja", "Administrador" }
                });

            migrationBuilder.InsertData(
                table: "produtos",
                columns: new[] { "Id", "Ativo", "CategoriaProdutoId", "Descricao", "DtAtualizacao", "DtCadastro", "Estoque", "Nome", "Preco" },
                values: new object[,]
                {
                    { 1, true, 1, "Caneca clara com mensagem positiva e visual minimalista para o dia a dia.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 18, "Caneca Be Happy", 59.90m },
                    { 2, true, 3, "Modelo com acabamento artesanal, textura marcada e estilo mais natural.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 12, "Caneca Ceramica Rustica", 54.90m },
                    { 3, true, 2, "Caneca escura com estampa de coala para quem gosta de pecas fofas e diferentes.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 15, "Caneca Coala", 64.90m },
                    { 4, true, 1, "Caneca em tom quente com detalhe de coracao para presentear com carinho.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 20, "Caneca Coracao", 62.90m },
                    { 5, true, 2, "Caneca com ilustracao de cachorro e acabamento divertido para uso diario.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 10, "Caneca Cachorro", 58.90m },
                    { 6, true, 1, "Caneca vermelha intensa, classica e versatil para cafe, cha ou chocolate.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 25, "Caneca Vermelha Cafe", 49.90m }
                });

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "Id", "Ativo", "DtAtualizacao", "DtCadastro", "Email", "NomeCompleto", "PerfilId", "Senha", "Telefone" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "ana.lopes@safermugs.com", "Ana Lopes", 1, "Seed@12345", "11990000001" },
                    { 2, true, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "bruno.costa@safermugs.com", "Bruno Costa", 1, "Seed@12345", "11990000002" },
                    { 3, true, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "carla.mendes@safermugs.com", "Carla Mendes", 1, "Seed@12345", "11990000003" },
                    { 4, true, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "diego.souza@safermugs.com", "Diego Souza", 1, "Seed@12345", "11990000004" },
                    { 5, true, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "elisa.martins@safermugs.com", "Elisa Martins", 1, "Seed@12345", "11990000005" },
                    { 6, true, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "felipe.rocha@safermugs.com", "Felipe Rocha", 1, "Seed@12345", "11990000006" },
                    { 7, true, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "marina.alves@safermugs.com", "Marina Alves", 2, "Seed@12345", "11990000007" }
                });

            migrationBuilder.InsertData(
                table: "avaliacoes",
                columns: new[] { "Id", "Comentario", "DtAtualizacao", "DtCadastro", "Nota", "ProdutoId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "Acabamento impecavel e o visual ficou exatamente como esperava.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 1, 1 },
                    { 2, "Linda e com boa qualidade, chegou muito bem embalada.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4, 1, 2 },
                    { 3, "Mensagem bem delicada e otima para presentear.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 1, 3 },
                    { 4, "Textura bonita e o estilo rustico da um charme extra.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4, 2, 2 },
                    { 5, "Peca muito bonita e com ar artesanal bem marcante.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 2, 4 },
                    { 6, "A estampa de coala ficou excelente e a caneca e bem resistente.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 3, 1 },
                    { 7, "Produto bonito e com acabamento muito caprichado.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4, 3, 5 },
                    { 8, "Visual apaixonante e otima escolha para presente.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 3, 6 },
                    { 9, "A ideia do coracao combinou muito com a cor da caneca.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 4, 3 },
                    { 10, "Bonita e delicada, atende bem quem gosta de pecas afetivas.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4, 4, 7 },
                    { 11, "A caneca do cachorro e divertida e tem uma pintura muito boa.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 5, 4 },
                    { 12, "Gostei bastante do formato e da proposta mais descontraida.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4, 5, 5 },
                    { 13, "Cor vibrante, pega muito bem e parece otima para o dia a dia.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 6, 1 },
                    { 14, "Modelo classico e elegante, combina com qualquer ambiente.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 4, 6, 2 },
                    { 15, "Perfeita para cafe, com cor intensa e acabamento uniforme.", new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), 5, 6, 6 }
                });

            migrationBuilder.InsertData(
                table: "imagens_produto",
                columns: new[] { "Id", "Legenda", "ProdutoId", "UrlImagem" },
                values: new object[,]
                {
                    { 1, "Caneca Be Happy", 1, "/imagens/mug_behappy.jpg" },
                    { 2, "Caneca Ceramica Rustica", 2, "/imagens/mug_ceramica_rustica.jpg" },
                    { 3, "Caneca Coala", 3, "/imagens/mug_coala.jpg" },
                    { 4, "Caneca Coracao", 4, "/imagens/mug_coracao.jpg" },
                    { 5, "Caneca cachorro", 5, "/imagens/mug_dogpan.jpg" },
                    { 6, "Caneca Vermelha Cafe", 6, "/imagens/mug_vermelha_cafe.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "imagens_produto",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "categorias_produto",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categorias_produto",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categorias_produto",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "perfis",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "perfis",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
