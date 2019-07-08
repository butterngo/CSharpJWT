using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpJWT.Migrations
{
    public partial class UpdateRefreshToken1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "RefreshTokens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                nullable: true);
        }
    }
}
