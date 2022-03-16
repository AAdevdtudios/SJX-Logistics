using Microsoft.EntityFrameworkCore.Migrations;

namespace SjxLogistics.Migrations
{
    public partial class draftschange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Refno",
                table: "Order",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Refno",
                table: "Order");
        }
    }
}
