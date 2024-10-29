using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZone.Migrations
{
    public partial class ContextUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamersGames_Games_GameId",
                table: "GamersGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_PublisherId",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_GamersGames_Games_GameId",
                table: "GamersGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_PublisherId",
                table: "Games",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamersGames_Games_GameId",
                table: "GamersGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_PublisherId",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_GamersGames_Games_GameId",
                table: "GamersGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_PublisherId",
                table: "Games",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
