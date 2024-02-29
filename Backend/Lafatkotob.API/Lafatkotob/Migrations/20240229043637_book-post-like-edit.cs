using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lafatkotob.Migrations
{
    /// <inheritdoc />
    public partial class bookpostlikeedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookPostLikes_UserId",
                table: "BookPostLikes");

            migrationBuilder.CreateIndex(
                name: "IX_BookPostLikes_UserId_BookId",
                table: "BookPostLikes",
                columns: new[] { "UserId", "BookId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookPostLikes_UserId_BookId",
                table: "BookPostLikes");

            migrationBuilder.CreateIndex(
                name: "IX_BookPostLikes_UserId",
                table: "BookPostLikes",
                column: "UserId");
        }
    }
}
