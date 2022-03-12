using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetAtlas.Migrations
{
    public partial class coriger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "etat",
                table: "Ressource");

            migrationBuilder.AddColumn<bool>(
                name: "etat",
                table: "Publication",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "etat",
                table: "Publication");

            migrationBuilder.AddColumn<bool>(
                name: "etat",
                table: "Ressource",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
