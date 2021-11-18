using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Orion.Api.Tests.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryTemp",
                columns: table => new
                {
                    InventoryId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaterialCode = table.Column<string>(maxLength: 32, nullable: false),
                    BranchFactory = table.Column<string>(maxLength: 32, nullable: false),
                    ZoneCode = table.Column<string>(maxLength: 32, nullable: false),
                    BatchCode = table.Column<string>(maxLength: 32, nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTemp", x => x.InventoryId);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceIssue",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoicePrefix = table.Column<string>(maxLength: 2, nullable: false),
                    InvoiceNum = table.Column<int>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    DeliveryCustCode = table.Column<string>(maxLength: 24, nullable: false),
                    DeliveryCustName = table.Column<string>(maxLength: 128, nullable: false),
                    Total = table.Column<decimal>(nullable: true),
                    CreateBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<int>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceIssue", x => x.InvoiceId);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceIssueItems",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(nullable: false),
                    DeliveryNum = table.Column<string>(maxLength: 20, nullable: false),
                    PurchaseNum = table.Column<string>(maxLength: 15, nullable: true),
                    Qty = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceIssueItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_InvoiceIssueItems_InvoiceIssue_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoiceIssue",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceIssueItems_InvoiceId",
                table: "InvoiceIssueItems",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryTemp");

            migrationBuilder.DropTable(
                name: "InvoiceIssueItems");

            migrationBuilder.DropTable(
                name: "InvoiceIssue");
        }
    }
}
