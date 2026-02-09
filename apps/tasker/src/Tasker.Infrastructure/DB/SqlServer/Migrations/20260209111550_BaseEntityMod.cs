using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Infrastructure.DB.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class BaseEntityMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActivatedAt",
                schema: "dbo",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ActivatedBy",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivatedBy",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivatedAt",
                schema: "dbo",
                table: "Issue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivatedBy",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                schema: "dbo",
                table: "Issue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivatedBy",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivatedBy",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ActivatedAt",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ActivatedBy",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "dbo",
                table: "Issue");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActivatedAt",
                schema: "dbo",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
