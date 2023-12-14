using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstapotAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageComentList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Images_ImageID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ImageID",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Images",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ImageID",
                table: "Comments",
                column: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Images_ImageID",
                table: "Comments",
                column: "ImageID",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
