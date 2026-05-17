using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassKeep.Migrations
{
    public partial class AddRoleToPKUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "PKUser",
                type: "TEXT",
                nullable: false,
                defaultValue: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "PKUser");
        }
    }
}
