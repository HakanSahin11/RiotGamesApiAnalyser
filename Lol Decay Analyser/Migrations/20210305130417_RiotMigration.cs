using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lol_Decay_Analyser.Migrations
{
    public partial class RiotMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Riots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SummonerName = table.Column<string>(nullable: true),
                    LastMatch = table.Column<DateTime>(nullable: true),
                    Rank = table.Column<string>(nullable: true),
                    TimeRemain = table.Column<int>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    RemainingGames = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riots", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Riots");
        }
    }
}
