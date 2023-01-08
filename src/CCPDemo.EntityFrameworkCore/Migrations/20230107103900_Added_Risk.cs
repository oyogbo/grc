using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCPDemo.Migrations
{
    public partial class Added_Risk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Risks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExistingControl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ERMRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskOwnerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualClosureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiskAccepted = table.Column<bool>(type: "bit", nullable: false),
                    RiskTypeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    RiskRatingId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risks_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Risks_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Risks_RiskRatings_RiskRatingId",
                        column: x => x.RiskRatingId,
                        principalTable: "RiskRatings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Risks_RiskTypes_RiskTypeId",
                        column: x => x.RiskTypeId,
                        principalTable: "RiskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Risks_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Risks_OrganizationUnitId",
                table: "Risks",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_RiskRatingId",
                table: "Risks",
                column: "RiskRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_RiskTypeId",
                table: "Risks",
                column: "RiskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_StatusId",
                table: "Risks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_UserId",
                table: "Risks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Risks");
        }
    }
}
