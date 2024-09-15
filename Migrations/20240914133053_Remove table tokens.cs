using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseAuth.Migrations
{
    /// <inheritdoc />
    public partial class Removetabletokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountUuid = table.Column<string>(type: "CHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int(11)", nullable: false),
                    Type = table.Column<int>(type: "int(11)", nullable: false),
                    Uuid = table.Column<string>(type: "CHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Account",
                        column: x => x.AccountUuid,
                        principalTable: "Accounts",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "FK_Token_Account",
                table: "Tokens",
                column: "AccountUuid");

            migrationBuilder.CreateIndex(
                name: "UQ_Token_Uuid",
                table: "Tokens",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Token_Value",
                table: "Tokens",
                column: "Value",
                unique: true);
        }
    }
}
