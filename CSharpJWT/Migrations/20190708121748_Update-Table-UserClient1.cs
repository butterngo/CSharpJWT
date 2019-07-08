using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpJWT.Migrations
{
    public partial class UpdateTableUserClient1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClients_Clients_UserId",
                table: "UserClients");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "UserClients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClients_ClientId",
                table: "UserClients",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClients_Clients_ClientId",
                table: "UserClients",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClients_Clients_ClientId",
                table: "UserClients");

            migrationBuilder.DropIndex(
                name: "IX_UserClients_ClientId",
                table: "UserClients");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "UserClients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClients_Clients_UserId",
                table: "UserClients",
                column: "UserId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
