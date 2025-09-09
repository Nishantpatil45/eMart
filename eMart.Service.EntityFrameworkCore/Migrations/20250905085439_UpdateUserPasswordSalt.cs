using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMart.Service.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPasswordSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSolt",
                table: "Users",
                newName: "PasswordSalt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "PasswordSolt");
        }
    }
}
