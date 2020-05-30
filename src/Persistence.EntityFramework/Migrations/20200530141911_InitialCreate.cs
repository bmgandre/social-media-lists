using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMediaLists.Persistence.EntityFramework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "SocialLists",
                columns: table => new
                {
                    SocialListId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLists", x => x.SocialListId);
                });

            migrationBuilder.CreateTable(
                name: "SocialAccount",
                columns: table => new
                {
                    SocialAccountId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Network = table.Column<string>(nullable: true),
                    AccoutName = table.Column<string>(nullable: true),
                    PersonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialAccount", x => x.SocialAccountId);
                    table.ForeignKey(
                        name: "FK_SocialAccount_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialListPerson",
                columns: table => new
                {
                    SocialListId = table.Column<long>(nullable: false),
                    PersonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialListPerson", x => new { x.SocialListId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_SocialListPerson_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialListPerson_SocialLists_SocialListId",
                        column: x => x.SocialListId,
                        principalTable: "SocialLists",
                        principalColumn: "SocialListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialAccount_PersonId",
                table: "SocialAccount",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialListPerson_PersonId",
                table: "SocialListPerson",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialAccount");

            migrationBuilder.DropTable(
                name: "SocialListPerson");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "SocialLists");
        }
    }
}