using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sonrysocialsapi.Migrations
{
    /// <inheritdoc />
    public partial class Attachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Attachment",
                table: "Posts",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Posts");
        }
    }
}
