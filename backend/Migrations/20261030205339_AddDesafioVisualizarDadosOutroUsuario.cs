using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioVisualizarDadosOutroUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Categoria", "Nome" },
                values: new object[] { "SecurityMisconfiguration", "DoS" });

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 32, "IDOR", "Consulte pela API os dados de uma conta diferente da sua.", "A página de perfil carrega os dados da conta por meio de uma requisição à API. O backend usa o identificador recebido na URL para buscar e retornar o perfil sem exigir autenticação nem verificar se o solicitante pode acessar aquele registro. Como o identificador é controlado pelo cliente, alterá-lo permite consultar informações pessoais de outras contas. A API deve validar a autorização no servidor para cada recurso; não basta a interface enviar o próprio identificador.", 1, "Visualizar dados de outro usuário", false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Categoria", "Nome" },
                values: new object[] { "XmlExternalEntity", "DOS" });
        }
    }
}
