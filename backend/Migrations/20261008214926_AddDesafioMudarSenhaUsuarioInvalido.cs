using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDesafioMudarSenhaUsuarioInvalido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "DescricaoDetalhes", "Dificuldade", "Nome", "Resolvido", "Rota" },
                values: new object[] { 23, "SecurityMisconfiguration", "Mude a senha do usuário inválido: |usuario.invalido@safemugs.com|.", "A funcionalidade de trocar senha na tela de perfil identifica o usuário a partir do JWT enviado pelo navegador. Como o segredo usado para assinar esse token foi exposto pelo serviço interno da porta 9090, é possível criar um token próprio e se passar por qualquer usuário. A descrição informa o e-mail do alvo, mas o método usa o identificador numérico para localizar o registro, por isso é preciso descobri-lo enumerando |GET /api/usuario/detalhes/{id}| com valores sequenciais até encontrar o e-mail procurado. Com esse identificador, basta gerar um token válido para o alvo, substituir o cookie atual e concluir a troca de senha pelo fluxo normal da tela de perfil chamando |PATCH /api/usuario/mudar-senha|.", 5, "Mude a senha do usuário inválido", false, "/perfil" });

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "Id", "DtAtualizacao", "DtCadastro", "Email", "EnderecoId", "HashSenha", "NomeCompleto", "PerfilId", "Telefone", "UrlImagemPerfil" },
                values: new object[] { 9, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "usuario.invalido@safemugs.com", null, "9c334b849a164533484ff4ca484645bb", "Usuario Inválido", 1, "11990000009", "/imagens/perfil/generic_profile.jpg" });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 48, 23, 1, "A troca de senha em /perfil identifica o usuário apenas pelos dados do JWT guardado no cookie safemugs.token." },
                    { 49, 23, 2, "Enumere os usuários pela rota GET /api/usuario/detalhes/{id} para descobrir o Id do usuário que deve ter a senha alterada." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 23);
        }
    }
}
