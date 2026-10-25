using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioEncontrarAdministracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 27, "Outros", "Acesse o painel de Administração do SafeMugs.", "O painel de Administração reúne funções fora do fluxo comum de compra. Quando uma conta possui perfil administrativo, a navegação disponibiliza uma entrada para esse painel. Explore a aplicação para encontrar o painel. O acesso, porém, exige uma conta com perfil de Administrador.", 2, "Acessar o painel de Administração", false, "/administracao" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 27);
        }
    }
}
