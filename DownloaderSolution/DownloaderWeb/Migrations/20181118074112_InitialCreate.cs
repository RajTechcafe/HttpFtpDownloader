using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DownloaderWeb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Downloads",
                columns: table => new
                {
                    DownloadId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    downloadUrl = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    downloadPercentage = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    TimeTaken = table.Column<TimeSpan>(nullable: false),
                    ParallelDownloads = table.Column<int>(nullable: false),
                    FileType = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Downloads", x => x.DownloadId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Downloads");
        }
    }
}
