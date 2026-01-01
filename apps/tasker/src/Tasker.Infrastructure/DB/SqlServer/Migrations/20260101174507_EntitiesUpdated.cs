using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Infrastructure.DB.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_User_AssignedId",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_User_UserId",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_AssignedId",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_Id_AssignerId_AssignedId",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "AssignedId",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.RenameColumn(
                name: "AssignedName",
                schema: "dbo",
                table: "Issue",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "AssignedImg",
                schema: "dbo",
                table: "Issue",
                newName: "UserImg");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId2",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issue_Id_AssignerId_UserId",
                schema: "dbo",
                table: "Issue",
                columns: new[] { "Id", "AssignerId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Issue_UserId2",
                schema: "dbo",
                table: "Issue",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_User_UserId",
                schema: "dbo",
                table: "Issue",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_User_UserId2",
                schema: "dbo",
                table: "Issue",
                column: "UserId2",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_User_UserId",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_User_UserId2",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_Id_AssignerId_UserId",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_UserId2",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "UserId2",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "dbo",
                table: "Issue",
                newName: "AssignedName");

            migrationBuilder.RenameColumn(
                name: "UserImg",
                schema: "dbo",
                table: "Issue",
                newName: "AssignedImg");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AssignedId",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_AssignedId",
                schema: "dbo",
                table: "Issue",
                column: "AssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_Id_AssignerId_AssignedId",
                schema: "dbo",
                table: "Issue",
                columns: new[] { "Id", "AssignerId", "AssignedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_User_AssignedId",
                schema: "dbo",
                table: "Issue",
                column: "AssignedId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_User_UserId",
                schema: "dbo",
                table: "Issue",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
