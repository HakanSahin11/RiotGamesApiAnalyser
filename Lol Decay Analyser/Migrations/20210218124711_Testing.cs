using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lol_Decay_Analyser.Migrations
{
    public partial class Testing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastMatch",
                table: "Riots",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Riots",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeRemain",
                table: "Riots",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMatch",
                table: "Riots");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Riots");

            migrationBuilder.DropColumn(
                name: "TimeRemain",
                table: "Riots");
        }
    }
}
