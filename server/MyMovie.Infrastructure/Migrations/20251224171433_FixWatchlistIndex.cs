using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMovie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixWatchlistIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_TmdbId_Type",
                table: "WatchlistItems");

            migrationBuilder.AddColumn<string>(
                name: "FirebaseUserId",
                table: "WatchlistItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_TmdbId_Type_FirebaseUserId",
                table: "WatchlistItems",
                columns: new[] { "TmdbId", "Type", "FirebaseUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_TmdbId_Type_FirebaseUserId",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "FirebaseUserId",
                table: "WatchlistItems");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_TmdbId_Type",
                table: "WatchlistItems",
                columns: new[] { "TmdbId", "Type" },
                unique: true);
        }
    }
}
