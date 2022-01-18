using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseStudy_Team2_Final.Migrations
{
    public partial class idea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Idea",
                columns: table => new
                {
                    IdeaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContributedBy = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Rating = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idea", x => x.IdeaID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Idea");
        }
    }
}
