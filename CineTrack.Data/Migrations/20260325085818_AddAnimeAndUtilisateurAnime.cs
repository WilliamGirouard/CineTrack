using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimeAndUtilisateurAnime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UtilisateurAnimes",
                columns: table => new
                {
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<int>(type: "INTEGER", nullable: true),
                    Commentaire = table.Column<string>(type: "TEXT", nullable: true),
                    DateAjout = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilisateurAnimes", x => new { x.UtilisateurId, x.AnimeId });
                    table.ForeignKey(
                        name: "FK_UtilisateurAnimes_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UtilisateurAnimes_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UtilisateurAnimes_AnimeId",
                table: "UtilisateurAnimes",
                column: "AnimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilisateurAnimes");
        }
    }
}
