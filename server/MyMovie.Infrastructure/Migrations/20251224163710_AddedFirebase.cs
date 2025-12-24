using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMovie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedFirebase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirebaseUserId",
                table: "WatchlistItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirebaseUserId",
                table: "WatchlistItems");
        }
    }
}
