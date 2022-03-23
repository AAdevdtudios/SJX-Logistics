using Microsoft.EntityFrameworkCore.Migrations;

namespace SjxLogistics.Migrations
{
    public partial class edits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecieverPhone",
                table: "Order",
                newName: "ReceiversPhone");

            migrationBuilder.RenameColumn(
                name: "CustormerEmail",
                table: "Order",
                newName: "CustomersEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiversPhone",
                table: "Order",
                newName: "RecieverPhone");

            migrationBuilder.RenameColumn(
                name: "CustomersEmail",
                table: "Order",
                newName: "CustormerEmail");
        }
    }
}
