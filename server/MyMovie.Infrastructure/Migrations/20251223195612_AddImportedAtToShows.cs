using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMovie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImportedAtToShows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ImportedAt",
                table: "TvShows",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportedAt",
                table: "TvShows");
        }
    }
}
