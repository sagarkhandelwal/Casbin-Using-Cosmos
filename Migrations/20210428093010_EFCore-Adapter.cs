using Microsoft.EntityFrameworkCore.Migrations;

namespace CasbinRBAC.Migrations
{
    public partial class EFCoreAdapter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CasbinRule");

            migrationBuilder.CreateTable(
                name: "casbin_rule",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ptype = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    v0 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    v1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    v2 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    v3 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    v4 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    v5 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_casbin_rule", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_casbin_rule_ptype",
                table: "casbin_rule",
                column: "ptype");

            migrationBuilder.CreateIndex(
                name: "IX_casbin_rule_v0",
                table: "casbin_rule",
                column: "v0");

            migrationBuilder.CreateIndex(
                name: "IX_casbin_rule_v1",
                table: "casbin_rule",
                column: "v1");

            migrationBuilder.CreateIndex(
                name: "IX_casbin_rule_v2",
                table: "casbin_rule",
                column: "v2");

            migrationBuilder.CreateIndex(
                name: "IX_casbin_rule_v3",
                table: "casbin_rule",
                column: "v3");

            migrationBuilder.CreateIndex(
                name: "IX_casbin_rule_v4",
                table: "casbin_rule",
                column: "v4");

            migrationBuilder.CreateIndex(
                name: "IX_casbin_rule_v5",
                table: "casbin_rule",
                column: "v5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "casbin_rule");

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
    }
}
