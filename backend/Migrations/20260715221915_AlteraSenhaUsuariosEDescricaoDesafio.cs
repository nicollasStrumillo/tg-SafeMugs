using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteraSenhaUsuariosEDescricaoDesafio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descricao", "Nome" },
                values: new object[] { "Acesse a conta de um dos usuários do domínio @safemugs.com utilizando força bruta.", "Brute force de login" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "ana.lopes@safemugs.com", "5858ea228cc2edf88721699b2c8638e5" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "bruno.costa@safemugs.com", "482c811da5d5b4bc6d497ffa98491e38" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "carla.mendes@safemugs.com", "37b4e2d82900d5e94b8da524fbeb33c0" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "diego.souza@safemugs.com", "cc25c0f861a83f5efadc6e1ba9d1269e" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "elisa.martins@safemugs.com", "3fc0a7acf087f549ac2b266baf94b8b1" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "felipe.rocha@safemugs.com", "0571749e2ac330a7455809c6b0e7af90" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "marina.alves@safemugs.com", "8afa847f50a716e64932d995c8e7435a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descricao", "Nome" },
                values: new object[] { "Utilize uma 'wordlist' para encontrar a senha de um usuário por força bruta.", "Brute force login" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "ana.lopes@safermugs.com", "1b13939cd7d77f68bac85931bfbb0a36" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "bruno.costa@safermugs.com", "1b13939cd7d77f68bac85931bfbb0a36" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "carla.mendes@safermugs.com", "1b13939cd7d77f68bac85931bfbb0a36" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "diego.souza@safermugs.com", "1b13939cd7d77f68bac85931bfbb0a36" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "elisa.martins@safermugs.com", "1b13939cd7d77f68bac85931bfbb0a36" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "felipe.rocha@safermugs.com", "1b13939cd7d77f68bac85931bfbb0a36" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "HashSenha" },
                values: new object[] { "marina.alves@safermugs.com", "1b13939cd7d77f68bac85931bfbb0a36" });
        }
    }
}
