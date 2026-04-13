using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFavorisImageAndTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtilisateurAnimes_Animes_AnimeId",
                table: "UtilisateurAnimes");

            migrationBuilder.DropIndex(
                name: "IX_UtilisateurAnimes_AnimeId",
                table: "UtilisateurAnimes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_MalId",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Favoris");

            migrationBuilder.DropColumn(
                name: "TitreAnime",
                table: "Favoris");

            migrationBuilder.RenameColumn(
                name: "AnimeId",
                table: "UtilisateurAnimes",
                newName: "MalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MalId",
                table: "UtilisateurAnimes",
                newName: "AnimeId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Favoris",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitreAnime",
                table: "Favoris",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
