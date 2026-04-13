using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixFavoris : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "PopularityScore",
                table: "Animes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Ratings",
                table: "Animes",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PopularityScore",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Ratings",
                table: "Animes");

            migrationBuilder.RenameColumn(
                name: "MalId",
                table: "UtilisateurAnimes",
                newName: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilisateurAnimes_AnimeId",
                table: "UtilisateurAnimes",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_MalId",
                table: "Animes",
                column: "MalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilisateurAnimes_Animes_AnimeId",
                table: "UtilisateurAnimes",
                column: "AnimeId",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
