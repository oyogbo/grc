using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCPDemo.Migrations
{
    public partial class Add_git_update_migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrganizationUnit",
                table: "KeyRiskIndicatorHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationUnit",
                table: "KeyRiskIndicatorHistory");
        }
    }
}
