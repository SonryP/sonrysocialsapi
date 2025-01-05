using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sonrysocialsapi.Migrations
{
    /// <inheritdoc />
    public partial class PostShareCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessCount",
                table: "Shares",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCount",
                table: "Shares");
        }
    }
}
