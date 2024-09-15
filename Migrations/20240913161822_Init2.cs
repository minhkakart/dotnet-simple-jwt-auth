using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseAuth.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Roles_Uuid",
                table: "Roles",
                column: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Roles_Uuid",
                table: "Roles");
        }
    }
}
