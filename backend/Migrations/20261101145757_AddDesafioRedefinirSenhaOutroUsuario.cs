using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioRedefinirSenhaOutroUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                column: "DescricaoDetalhes",
                value: "O fluxo de login do SafeMugs não implementa nenhum mecanismo de antiautomação: não há limite de tentativas por conta, bloqueio temporário, captcha nem atraso entre requisições. Conhecendo um e-mail válido do domínio @safemugs.com, é possível automatizar tentativas consecutivas de senha até acertar. As senhas dos usuários comuns são fracas e baseadas em padrões conhecidos (como 'welcome123' ou 'password123'), o que torna viável um ataque usando uma lista de palavras. Sem qualquer controle de velocidade, um script consegue testar milhares de combinações em poucos segundos, obtendo acesso a uma conta legítima.");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 33, "BrokenAntiAutomation", "Use o fluxo de recuperação de senha para redefinir a senha de uma conta comum pré-cadastrada do domínio @safemugs.com.", "A recuperação envia um link com um token para o e-mail associado à conta. Tokens de redefinição precisam ser imprevisíveis, e o endpoint deve limitar tentativas. Neste fluxo, o token incorpora o identificador do usuário e tem poucas combinações variáveis, permitindo testar candidatos automaticamente e redefinir a senha sem acesso à caixa de entrada do alvo.", 3, "Redefinir senha de outro usuário", false, "/senha" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 64, 33, 1, "Solicite um link de recuperação para uma conta que você controla e compare o parâmetro token em mais de uma solicitação." },
                    { 65, 33, 2, "Parte do token acompanha o identificador da conta-alvo; os caracteres restantes têm poucas possibilidades. Considere automatizar candidatos no POST que conclui a recuperação." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                column: "DescricaoDetalhes",
                value: "O fluxo de login do SafeMugs não implementa nenhum mecanismo de antiautomação: não há limite de tentativas por conta, bloqueio temporário, captcha nem atraso entre requisições. Conhecendo um e-mail válido do domínio |@safemugs.com|, é possível automatizar tentativas consecutivas de senha até acertar. As senhas dos usuários comuns são fracas e baseadas em padrões conhecidos (como 'welcome123' ou 'password123'), o que torna viável um ataque usando uma lista de palavras. Sem qualquer controle de velocidade, um script consegue testar milhares de combinações em poucos segundos, obtendo acesso a uma conta legítima.");
        }
    }
}
