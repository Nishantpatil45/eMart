using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMart.Service.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addedenhancedAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserOtps",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "UserOtps",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "UserOtps",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserOtps",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserOtps");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "UserOtps");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "UserOtps");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserOtps");
        }
    }
}
