using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMart.Service.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categorys",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categorys");
        }
    }
}
