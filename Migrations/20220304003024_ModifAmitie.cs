using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetAtlas.Migrations
{
    public partial class ModifAmitie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amitie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSender = table.Column<int>(type: "int", nullable: false),
                    IdReceiver = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amitie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amitie_Membre_IdReceiver",
                        column: x => x.IdReceiver,
                        principalTable: "Membre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Amitie_IdReceiver",
                table: "Amitie",
                column: "IdReceiver");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amitie");
        }
    }
}
