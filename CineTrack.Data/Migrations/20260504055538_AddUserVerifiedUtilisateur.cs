using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserVerifiedUtilisateur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserVerified",
                table: "Utilisateurs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserVerified",
                table: "Utilisateurs");
        }
    }
}
