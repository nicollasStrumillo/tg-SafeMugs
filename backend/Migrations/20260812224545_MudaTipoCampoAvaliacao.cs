using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MudaTipoCampoAvaliacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Nota",
                table: "avaliacoes",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nota",
                value: 5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nota",
                value: 4.5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nota",
                value: 5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nota",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Nota",
                value: 3.5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Nota",
                value: 4.5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Nota",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Nota",
                value: 5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Nota",
                value: 3.5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Nota",
                value: 4.5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Nota",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Nota",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nota",
                value: 5f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Nota",
                value: 4.4f);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Nota",
                value: 4.5f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Nota",
                table: "avaliacoes",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nota",
                value: 4);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nota",
                value: 4);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Nota",
                value: 4);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Nota",
                value: 4);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Nota",
                value: 4);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nota",
                value: 5);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Nota",
                value: 4);

            migrationBuilder.UpdateData(
                table: "avaliacoes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Nota",
                value: 5);
        }
    }
}
