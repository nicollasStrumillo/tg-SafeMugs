using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioForjarJwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 31, "SecurityMisconfiguration", "Usando uma conta comum, faça uma requisição bem-sucedida a uma operação exclusiva para administradores, sem utilizar uma sessão válida de administrador.", "Um JWT contém claims que representam a identidade e as permissões do usuário. Seu payload pode ser lido pelo cliente, mas a assinatura é necessária para detectar alterações e confirmar que essas informações são confiáveis. Se a API usa uma claim de perfil para autorizar uma operação antes de validar a assinatura, dados alterados pelo cliente podem ser interpretados como privilégios legítimos. A proteção de navegação no cliente não substitui a autorização no servidor: a API deve validar o token antes de confiar nas claims.", 3, "Acesso reservado", false, null });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 61, 31, 1, "Observe os dados de autenticação que o cliente envia junto à requisição." },
                    { 62, 31, 2, "A API recebe um JWT. Inspecione o payload e procure a claim de perfil usada na decisão de acesso." },
                    { 63, 31, 3, "Alterar uma claim modifica o conteúdo do token e invalida sua assinatura original. Verifique se o servidor valida essa assinatura antes de confiar no perfil." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 31);
        }
    }
}
