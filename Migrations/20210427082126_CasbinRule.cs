using Microsoft.EntityFrameworkCore.Migrations;

namespace CasbinRBAC.Migrations
{
    public partial class CasbinRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CasbinRule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    V0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    V1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    V2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    V3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    V4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    V5 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasbinRule", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CasbinRule");
        }
    }
}
