using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class _17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestingUserName",
                table: "Invitations",
                newName: "ToPlayerName");

            migrationBuilder.RenameColumn(
                name: "RequestingUserId",
                table: "Invitations",
                newName: "ToUserId");

            migrationBuilder.RenameColumn(
                name: "InvitedUserName",
                table: "Invitations",
                newName: "FromPlayerName");

            migrationBuilder.RenameColumn(
                name: "InvitedRoomGuid",
                table: "Invitations",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "InvitedGuid",
                table: "Invitations",
                newName: "FromPlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToUserId",
                table: "Invitations",
                newName: "RequestingUserId");

            migrationBuilder.RenameColumn(
                name: "ToPlayerName",
                table: "Invitations",
                newName: "RequestingUserName");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Invitations",
                newName: "InvitedRoomGuid");

            migrationBuilder.RenameColumn(
                name: "FromPlayerName",
                table: "Invitations",
                newName: "InvitedUserName");

            migrationBuilder.RenameColumn(
                name: "FromPlayerId",
                table: "Invitations",
                newName: "InvitedGuid");
        }
    }
}
