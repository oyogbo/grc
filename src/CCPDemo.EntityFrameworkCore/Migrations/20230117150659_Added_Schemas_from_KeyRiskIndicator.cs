using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCPDemo.Migrations
{
    public partial class Added_Schemas_from_KeyRiskIndicator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateCreated",
                table: "KeyRiskIndicatorHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "KeyRiskIndicatorHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReviewStatus",
                table: "KeyRiskIndicatorHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "KeyRiskIndicatorHistory");

            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "KeyRiskIndicatorHistory");

            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "KeyRiskIndicatorHistory");
        }
    }
}
