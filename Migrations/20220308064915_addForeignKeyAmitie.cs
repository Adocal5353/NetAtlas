using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetAtlas.Migrations
{
    public partial class addForeignKeyAmitie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Amitie_IdSender",
                table: "Amitie",
                column: "IdSender");

            migrationBuilder.AddForeignKey(
                name: "FK_Amitie_Membre_IdSender",
                table: "Amitie",
                column: "IdSender",
                principalTable: "Membre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amitie_Membre_IdSender",
                table: "Amitie");

            migrationBuilder.DropIndex(
                name: "IX_Amitie_IdSender",
                table: "Amitie");
        }
    }
}
