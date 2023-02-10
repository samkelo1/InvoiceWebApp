using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceWebApp.Migrations
{
    /// <inheritdoc />
    public partial class initcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    Number = table.Column<int>(type: "int", nullable: false),
                    PostalCode = table.Column<string>(type: "VARCHAR(40)", nullable: false),
                    Street = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Suffix = table.Column<string>(type: "VARCHAR(10)", nullable: true),
                    City = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Country = table.Column<string>(type: "VARCHAR(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => new { x.Number, x.PostalCode });
                });

            migrationBuilder.CreateTable(
                name: "debtors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    LastName = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    CompanyName = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    Email = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    BankAccount = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Phone = table.Column<string>(type: "VARCHAR(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_debtors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "VARCHAR(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "VARCHAR(300)", nullable: false),
                    Website = table.Column<string>(type: "VARCHAR(300)", nullable: true),
                    Phone = table.Column<string>(type: "VARCHAR(40)", nullable: true),
                    Email = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(80)", nullable: false),
                    SMTP = table.Column<string>(type: "VARCHAR(80)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    PostalCode = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    City = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    Country = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    BankAccount = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Bank = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    BusinessNumber = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    VAT = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    InvoicePrefix = table.Column<string>(type: "VARCHAR(20)", nullable: true),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowLogo = table.Column<bool>(type: "bit", nullable: false),
                    ShowLogoInPDF = table.Column<bool>(type: "bit", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(150)", nullable: true),
                    LastName = table.Column<string>(type: "VARCHAR(175)", nullable: true),
                    CompanyName = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "debtor_has_addresses",
                columns: table => new
                {
                    DebtorId = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    PostalCode = table.Column<string>(type: "VARCHAR(40)", nullable: false),
                    Number = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_debtor_has_addresses", x => new { x.DebtorId, x.PostalCode, x.Number });
                    table.ForeignKey(
                        name: "dha_address_fk",
                        columns: x => new { x.Number, x.PostalCode },
                        principalTable: "addresses",
                        principalColumns: new[] { "Number", "PostalCode" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "dha_debtor_fk",
                        column: x => x.DebtorId,
                        principalTable: "debtors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    InvoiceNumber = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    CustomerId = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpiredOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    discount = table.Column<int>(type: "INT", nullable: false),
                    Comment = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Concept = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.InvoiceNumber);
                    table.ForeignKey(
                        name: "invoices_debtors_fk",
                        column: x => x.CustomerId,
                        principalTable: "debtors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_items",
                columns: table => new
                {
                    InvoiceNumber = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    ItemNumber = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    Price = table.Column<decimal>(type: "DECIMAL(18,0)", nullable: false),
                    Tax = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_items", x => new { x.ItemNumber, x.InvoiceNumber });
                    table.ForeignKey(
                        name: "items_invoice_fk",
                        column: x => x.InvoiceNumber,
                        principalTable: "invoices",
                        principalColumn: "InvoiceNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_debtor_has_addresses_Number_PostalCode",
                table: "debtor_has_addresses",
                columns: new[] { "Number", "PostalCode" });

            migrationBuilder.CreateIndex(
                name: "IX_debtors_Id",
                table: "debtors",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoice_items_InvoiceNumber",
                table: "invoice_items",
                column: "InvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_CustomerId",
                table: "invoices",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "debtor_has_addresses");

            migrationBuilder.DropTable(
                name: "invoice_items");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "debtors");
        }
    }
}
