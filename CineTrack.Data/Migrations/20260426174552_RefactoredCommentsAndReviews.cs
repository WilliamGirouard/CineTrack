using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredCommentsAndReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "UtilisateurAnimes");

            migrationBuilder.DropColumn(
                name: "AnimeId",
                table: "Commentaires");

            migrationBuilder.RenameColumn(
                name: "AnimeId",
                table: "Favoris",
                newName: "MalId");

            migrationBuilder.RenameColumn(
                name: "IsCurrentUserAuthor",
                table: "Commentaires",
                newName: "MalId");

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: false),
                    MalId = table.Column<long>(type: "INTEGER", nullable: false),
                    NoteUtilisateur = table.Column<int>(type: "INTEGER", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UtilisateurId",
                table: "Notes",
                column: "UtilisateurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.RenameColumn(
                name: "MalId",
                table: "Favoris",
                newName: "AnimeId");

            migrationBuilder.RenameColumn(
                name: "MalId",
                table: "Commentaires",
                newName: "IsCurrentUserAuthor");

            migrationBuilder.AddColumn<int>(
                name: "AnimeId",
                table: "Commentaires",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MalId = table.Column<int>(type: "INTEGER", nullable: true),
                    PopularityScore = table.Column<int>(type: "INTEGER", nullable: false),
                    Ratings = table.Column<double>(type: "REAL", nullable: false),
                    Season = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Synopsis = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    TitleEnglish = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UtilisateurAnimes",
                columns: table => new
                {
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: false),
                    MalId = table.Column<int>(type: "INTEGER", nullable: false),
                    Commentaire = table.Column<string>(type: "TEXT", nullable: true),
                    DateAjout = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Note = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilisateurAnimes", x => new { x.UtilisateurId, x.MalId });
                    table.ForeignKey(
                        name: "FK_UtilisateurAnimes_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
