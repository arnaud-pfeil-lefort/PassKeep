using Microsoft.EntityFrameworkCore.Migrations;
using PassKeep.ClassesGenerales;

#nullable disable

namespace PassKeep.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuperAdmin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ClassFonctionsGenerales.CreerUtilisateurSuperAdmin(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
