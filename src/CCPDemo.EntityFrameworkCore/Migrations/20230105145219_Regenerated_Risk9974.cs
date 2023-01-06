using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCPDemo.Migrations
{
    public partial class Regenerated_Risk9974 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskTypeId",
                table: "Risks");

            migrationBuilder.AddColumn<string>(
                name: "RiskType",
                table: "Risks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskType",
                table: "Risks");

            migrationBuilder.AddColumn<int>(
                name: "RiskTypeId",
                table: "Risks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
