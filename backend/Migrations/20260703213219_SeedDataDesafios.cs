using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataDesafios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Dificuldade",
                table: "desafios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "desafios",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "desafios",
                columns: new[] { "Id", "Categoria", "Descricao", "Dificuldade", "Nome", "UrlMitigacao" },
                values: new object[,]
                {
                    { 1, "SqlInjection", "Acesse a conta administrativa usando o fluxo de login.", 3, "Login como Admin", "url_placeholder" },
                    { 2, "ExcessiveDataExposure", "Enumere alguns e-mails de usuários existentes.", 2, "Encontrar usuários válidos", "url_placeholder" },
                    { 3, "BrokenAntiAutomation", "Utilize uma 'wordlist' para encontrar a senha de um usuário por força bruta.", 2, "Brute force login", "url_placeholder" },
                    { 4, "ImproperInputValidation", "Teste a validação do formulário de cadastro.", 2, "Validar cadastro", "url_placeholder" },
                    { 5, "ParameterTampering", "Altere os dados enviados antes que eles cheguem ao servidor e crie uma conta de administrador.", 3, "Manipular cadastro", "url_placeholder" },
                    { 6, "SqlInjection", "Use a busca do catálogo para injetar uma consulta.", 3, "Buscar por SQL Injection", "url_placeholder" },
                    { 7, "ReflectedXSS", "Explore a busca do catálogo com payload de XSS refletido.", 2, "Buscar com script", "url_placeholder" },
                    { 8, "ParameterTampering", "Modifique os parâmetros de filtro e ordenação da listagem para encontrar informações sensíveis.", 2, "Alterar ordenação do catálogo", "url_placeholder" },
                    { 9, "StoredXSS", "Insira conteúdo malicioso nos comentários do produto.", 3, "Comentar com HTML", "url_placeholder" },
                    { 10, "IDOR", "Escreva um comentário que não pertence ao seu usuário.", 3, "Criar um comentário por outro usuário", "url_placeholder" },
                    { 11, "BrokenAuthentication", "Explore o fluxo de esqueci minha senha sem proteção suficiente.", 3, "Recuperar senha insegura", "url_placeholder" },
                    { 12, "Outros", "Localize a página de desafios por enumeração de diretórios.", 1, "Encontrar a Score-Board", "url_placeholder" }
                });

            migrationBuilder.InsertData(
                table: "dicas_desafio",
                columns: new[] { "Id", "DesafioId", "NrDica", "Texto" },
                values: new object[,]
                {
                    { 1, 1, 1, "Primeiro, identifique qual é o e-mail do administrador." },
                    { 2, 1, 2, "A falha está no fluxo de autenticação." },
                    { 3, 1, 3, "Teste SQL Injection em entradas no campo de e-mail ou senha." },
                    { 4, 2, 1, "O desafio pode ser resolvido na página de detalhes do produto." },
                    { 5, 2, 2, "Observe o retorno da chamada que lista comentários do produto." },
                    { 6, 3, 1, "Não há bloqueio por muitas tentativas repetidas por minuto." },
                    { 7, 3, 2, "Encontre um e-mail de usuário válido para atacar." },
                    { 8, 4, 1, "Observe quais campos aceitam valores inesperados." },
                    { 9, 4, 2, "A validação dos campos pode ser insuficiente." },
                    { 10, 5, 1, "Primeiro, identifique um campo que compõe um usuário mas não devia ser enviado pelo formulário de cadastro." },
                    { 11, 5, 2, "O payload pode ser alterado antes de chegar ao servidor." },
                    { 12, 6, 1, "A busca do catálogo é a superfície de ataque." },
                    { 13, 6, 2, "O nome do desafio aponta para SQL Injection." },
                    { 14, 7, 1, "A busca reflete sua entrada na interface de resposta." },
                    { 15, 7, 2, "Teste inserir um payload malicioso no termo de pesquisa." },
                    { 16, 8, 1, "Filtros e ordenação costumam vir por query string." },
                    { 17, 8, 2, "O backend pode estar ordenando por qualquer parâmetro que ele receber." },
                    { 18, 9, 1, "O desafio está na página de detalhes do produto." },
                    { 19, 9, 2, "Comentários podem ser renderizados sem sanitização." },
                    { 20, 10, 1, "O payload enviado ao backend pode estar transmitindo dados de forma insegura." },
                    { 21, 10, 2, "Tente interceptar e modificar a requisição." },
                    { 22, 11, 1, "Dica 1 placeholder" },
                    { 23, 11, 2, "Dica 2 placeholder" }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Desafio_Dificuldade",
                table: "desafios",
                sql: "Dificuldade >= 1 AND Dificuldade <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Desafio_Dificuldade",
                table: "desafios");

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "dicas_desafio",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "desafios");

            migrationBuilder.AlterColumn<string>(
                name: "Dificuldade",
                table: "desafios",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
