using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetAtlas.Migrations
{
    public partial class UpdatePublication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publication_Ressource_IdRessource",
                table: "Publication");

            migrationBuilder.DropIndex(
                name: "IX_Publication_IdRessource",
                table: "Publication");

            migrationBuilder.DropColumn(
                name: "IdRessource",
                table: "Publication");

            migrationBuilder.AddColumn<string>(
                name: "Chemin",
                table: "Ressource",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "IdPublication",
                table: "Ressource",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ressource_IdPublication",
                table: "Ressource",
                column: "IdPublication");

            migrationBuilder.AddForeignKey(
                name: "FK_Ressource_Publication_IdPublication",
                table: "Ressource",
                column: "IdPublication",
                principalTable: "Publication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ressource_Publication_IdPublication",
                table: "Ressource");

            migrationBuilder.DropIndex(
                name: "IX_Ressource_IdPublication",
                table: "Ressource");

            migrationBuilder.DropColumn(
                name: "Chemin",
                table: "Ressource");

            migrationBuilder.DropColumn(
                name: "IdPublication",
                table: "Ressource");

            migrationBuilder.AddColumn<int>(
                name: "IdRessource",
                table: "Publication",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Publication_IdRessource",
                table: "Publication",
                column: "IdRessource");

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_Ressource_IdRessource",
                table: "Publication",
                column: "IdRessource",
                principalTable: "Ressource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
