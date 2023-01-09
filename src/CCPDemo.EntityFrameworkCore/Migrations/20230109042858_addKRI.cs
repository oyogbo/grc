using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCPDemo.Migrations
{
    public partial class addKRI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRIId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyRiskIndicatorHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalRecord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BussinessLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyRiskIndicatorHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyRiskIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessLines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Process = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubProcess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PotentialRisk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikelihoodOfOccurrence_irr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikelihoodOfImpact_irr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyControl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsControlInUse = table.Column<bool>(type: "bit", nullable: false),
                    ControlEffectiveness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikelihoodOfOccurrence_rrr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikelihoodOfImpact_rrr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MitigationPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyRiskIndicators", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "KeyRiskIndicatorHistory");

            migrationBuilder.DropTable(
                name: "KeyRiskIndicators");
        }
    }
}
