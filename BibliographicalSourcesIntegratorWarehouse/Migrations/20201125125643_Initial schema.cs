using Microsoft.EntityFrameworkCore.Migrations;

namespace BibliographicalSourcesIntegratorWarehouse.Migrations
{
    public partial class Initialschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surnames = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exemplars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Volume = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    JournalId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exemplars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exemplars_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    InitialPage = table.Column<string>(nullable: true),
                    FinalPage = table.Column<string>(nullable: true),
                    ExemplarId = table.Column<int>(nullable: true),
                    Editorial = table.Column<string>(nullable: true),
                    Congress = table.Column<string>(nullable: true),
                    Edition = table.Column<string>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    CongressComunication_InitialPage = table.Column<string>(nullable: true),
                    CongressComunication_FinalPage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_Exemplars_ExemplarId",
                        column: x => x.ExemplarId,
                        principalTable: "Exemplars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Person_Publication",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(nullable: false),
                    PublicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person_Publication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Publication_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person_Publication_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exemplars_JournalId",
                table: "Exemplars",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Publication_PersonId",
                table: "Person_Publication",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Publication_PublicationId",
                table: "Person_Publication",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_ExemplarId",
                table: "Publications",
                column: "ExemplarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person_Publication");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "Exemplars");

            migrationBuilder.DropTable(
                name: "Journals");
        }
    }
}
