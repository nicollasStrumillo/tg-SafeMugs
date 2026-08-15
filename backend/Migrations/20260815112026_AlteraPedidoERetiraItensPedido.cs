using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlteraPedidoERetiraItensPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "itens_pedido");

            migrationBuilder.DropColumn(
                name: "DtCadastro",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "DtEntregue",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "DtEnviado",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "DtPagamento",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "MetodoPagamento",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "pedidos");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroPedido",
                table: "pedidos",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CarrinhoId",
                table: "pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeItens",
                table: "pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_CarrinhoId",
                table: "pedidos",
                column: "CarrinhoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_pedidos_carrinhos_CarrinhoId",
                table: "pedidos",
                column: "CarrinhoId",
                principalTable: "carrinhos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pedidos_carrinhos_CarrinhoId",
                table: "pedidos");

            migrationBuilder.DropIndex(
                name: "IX_pedidos_CarrinhoId",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "CarrinhoId",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "QuantidadeItens",
                table: "pedidos");

            migrationBuilder.AlterColumn<int>(
                name: "NumeroPedido",
                table: "pedidos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DtCadastro",
                table: "pedidos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DtEntregue",
                table: "pedidos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DtEnviado",
                table: "pedidos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DtPagamento",
                table: "pedidos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPagamento",
                table: "pedidos",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "pedidos",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itens_pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itens_pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_itens_pedido_pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_itens_pedido_produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_itens_pedido_PedidoId_ProdutoId",
                table: "itens_pedido",
                columns: new[] { "PedidoId", "ProdutoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_itens_pedido_ProdutoId",
                table: "itens_pedido",
                column: "ProdutoId");
        }
    }
}
