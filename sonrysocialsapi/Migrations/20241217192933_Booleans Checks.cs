using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sonrysocialsapi.Migrations
{
    /// <inheritdoc />
    public partial class BooleansChecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "Likes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "Likes");
        }
    }
}
