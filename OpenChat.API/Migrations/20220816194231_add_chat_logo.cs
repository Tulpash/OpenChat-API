using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenChat.API.Migrations
{
    public partial class add_chat_logo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqueName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "UniqueName",
                table: "AspNetUsers");
        }
    }
}
