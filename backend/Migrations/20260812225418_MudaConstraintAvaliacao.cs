using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MudaConstraintAvaliacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Avaliacao_Nota",
                table: "avaliacoes");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Avaliacao_Nota",
                table: "avaliacoes",
                sql: "Nota >= 0 AND Nota <= 5.0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Avaliacao_Nota",
                table: "avaliacoes");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Avaliacao_Nota",
                table: "avaliacoes",
                sql: "Nota >= 1 AND Nota <= 5");
        }
    }
}
