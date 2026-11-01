using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteraDificuldadeDesafios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Dificuldade",
                value: 1);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Dificuldade",
                value: 3);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 13,
                column: "Dificuldade",
                value: 1);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 26,
                column: "Dificuldade",
                value: 5);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 28,
                column: "Dificuldade",
                value: 5);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 31,
                column: "Dificuldade",
                value: 4);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 33,
                column: "Dificuldade",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Dificuldade",
                value: 2);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Dificuldade",
                value: 2);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 13,
                column: "Dificuldade",
                value: 2);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 26,
                column: "Dificuldade",
                value: 4);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 28,
                column: "Dificuldade",
                value: 4);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 31,
                column: "Dificuldade",
                value: 3);

            migrationBuilder.UpdateData(
                table: "desafios",
                keyColumn: "Id",
                keyValue: 33,
                column: "Dificuldade",
                value: 3);
        }
    }
}
