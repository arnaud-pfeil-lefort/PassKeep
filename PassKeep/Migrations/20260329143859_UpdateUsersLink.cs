using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassKeep.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsersLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeProfilConnexion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeProfilConnexion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfilConnexion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ServiceName = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceLogin = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceCryptPassword = table.Column<string>(type: "TEXT", nullable: false),
                    PKUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TypeProfilConnexionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilConnexion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilConnexion_PKUser_PKUserId",
                        column: x => x.PKUserId,
                        principalTable: "PKUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfilConnexion_TypeProfilConnexion_TypeProfilConnexionId",
                        column: x => x.TypeProfilConnexionId,
                        principalTable: "TypeProfilConnexion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilConnexion_PKUserId",
                table: "ProfilConnexion",
                column: "PKUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilConnexion_TypeProfilConnexionId",
                table: "ProfilConnexion",
                column: "TypeProfilConnexionId");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilConnexion");
        }
    }
}
