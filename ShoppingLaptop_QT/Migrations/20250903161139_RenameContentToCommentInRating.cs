using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingLaptop_QT.Migrations
{
    /// <inheritdoc />
    public partial class RenameContentToCommentInRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_ProductId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Ratings",
                newName: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductId",
                table: "Ratings",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_ProductId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Ratings",
                newName: "Content");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductId",
                table: "Ratings",
                column: "ProductId");
        }
    }
}
