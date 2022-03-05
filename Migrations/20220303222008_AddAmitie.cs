using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetAtlas.Migrations
{
    public partial class AddAmitie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publication_Membre_idMemdre",
                table: "Publication");

            migrationBuilder.DropForeignKey(
                name: "FK_Publication_Ressource_idRessource",
                table: "Publication");

            migrationBuilder.RenameColumn(
                name: "idRessource",
                table: "Publication",
                newName: "IdRessource");

            migrationBuilder.RenameColumn(
                name: "idMemdre",
                table: "Publication",
                newName: "IdMemdre");

            migrationBuilder.RenameIndex(
                name: "IX_Publication_idRessource",
                table: "Publication",
                newName: "IX_Publication_IdRessource");

            migrationBuilder.RenameIndex(
                name: "IX_Publication_idMemdre",
                table: "Publication",
                newName: "IX_Publication_IdMemdre");

            migrationBuilder.RenameColumn(
                name: "AdminPassword",
                table: "Admin",
                newName: "Prenom");

            migrationBuilder.RenameColumn(
                name: "AdminName",
                table: "Admin",
                newName: "Password");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Ressource",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "isLogged",
                table: "Moderateur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "isLogged",
                table: "Membre",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admin",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Admin",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "isLogged",
                table: "Admin",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_Membre_IdMemdre",
                table: "Publication",
                column: "IdMemdre",
                principalTable: "Membre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_Ressource_IdRessource",
                table: "Publication",
                column: "IdRessource",
                principalTable: "Ressource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publication_Membre_IdMemdre",
                table: "Publication");

            migrationBuilder.DropForeignKey(
                name: "FK_Publication_Ressource_IdRessource",
                table: "Publication");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Ressource");

            migrationBuilder.DropColumn(
                name: "isLogged",
                table: "Moderateur");

            migrationBuilder.DropColumn(
                name: "isLogged",
                table: "Membre");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "isLogged",
                table: "Admin");

            migrationBuilder.RenameColumn(
                name: "IdRessource",
                table: "Publication",
                newName: "idRessource");

            migrationBuilder.RenameColumn(
                name: "IdMemdre",
                table: "Publication",
                newName: "idMemdre");

            migrationBuilder.RenameIndex(
                name: "IX_Publication_IdRessource",
                table: "Publication",
                newName: "IX_Publication_idRessource");

            migrationBuilder.RenameIndex(
                name: "IX_Publication_IdMemdre",
                table: "Publication",
                newName: "IX_Publication_idMemdre");

            migrationBuilder.RenameColumn(
                name: "Prenom",
                table: "Admin",
                newName: "AdminPassword");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Admin",
                newName: "AdminName");

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_Membre_idMemdre",
                table: "Publication",
                column: "idMemdre",
                principalTable: "Membre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_Ressource_idRessource",
                table: "Publication",
                column: "idRessource",
                principalTable: "Ressource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
