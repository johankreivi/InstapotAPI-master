using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstapotAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedUpdateLoginStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LoginStatus",
                table: "Profiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginStatus",
                table: "Profiles");
        }
    }
}
