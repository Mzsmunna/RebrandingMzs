using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Infrastructure.DB.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class BaseUserUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                schema: "dbo",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Roles",
                schema: "dbo",
                table: "User",
                newName: "Covr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Covr",
                schema: "dbo",
                table: "User",
                newName: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                schema: "dbo",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
