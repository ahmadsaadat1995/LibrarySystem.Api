using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Books_BookId",
                table: "BookPublishers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherId",
                table: "BookPublishers");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authers_AutherId",
                table: "Books");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Books_BookId",
                table: "BookPublishers",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherId",
                table: "BookPublishers",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authers_AutherId",
                table: "Books",
                column: "AutherId",
                principalTable: "Authers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Books_BookId",
                table: "BookPublishers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherId",
                table: "BookPublishers");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authers_AutherId",
                table: "Books");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Books_BookId",
                table: "BookPublishers",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherId",
                table: "BookPublishers",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authers_AutherId",
                table: "Books",
                column: "AutherId",
                principalTable: "Authers",
                principalColumn: "Id");
        }
    }
}
