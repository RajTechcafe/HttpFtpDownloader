using Microsoft.EntityFrameworkCore.Migrations;

namespace DownloaderWeb.Migrations
{
    public partial class ContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Downloads",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Downloads");
        }
    }
}
