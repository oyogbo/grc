using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCPDemo.Migrations
{
    public partial class Added_ManagementReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagementReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    risk_id = table.Column<int>(type: "int", nullable: false),
                    submission_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    review = table.Column<int>(type: "int", nullable: false),
                    reviewer = table.Column<int>(type: "int", nullable: false),
                    next_step = table.Column<int>(type: "int", nullable: false),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    next_review = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementReviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagementReviews_TenantId",
                table: "ManagementReviews",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagementReviews");
        }
    }
}
