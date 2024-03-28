using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lafatkotob.Migrations
{
    /// <inheritdoc />
    public partial class fixedconversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastMessage",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessage",
                table: "Conversations");
        }
    }
}
