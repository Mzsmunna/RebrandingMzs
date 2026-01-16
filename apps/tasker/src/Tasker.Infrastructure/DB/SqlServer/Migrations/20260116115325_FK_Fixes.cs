using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Infrastructure.DB.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class FK_Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_User_UserId1",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_User_UserId2",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_UserId1",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_UserId2",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "UserId2",
                schema: "dbo",
                table: "Issue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId2",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issue_UserId1",
                schema: "dbo",
                table: "Issue",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_UserId2",
                schema: "dbo",
                table: "Issue",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_User_UserId1",
                schema: "dbo",
                table: "Issue",
                column: "UserId1",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_User_UserId2",
                schema: "dbo",
                table: "Issue",
                column: "UserId2",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
