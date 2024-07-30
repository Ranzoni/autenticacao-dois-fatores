using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutenticacaoDoisFatores.Infra.Migrations
{
    /// <inheritdoc />
    public partial class emailuniqueentidadeacesso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EntidadesAcesso_Chave",
                table: "EntidadesAcesso");

            migrationBuilder.DropIndex(
                name: "IX_EntidadesAcesso_Email",
                table: "EntidadesAcesso");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadesAcesso_Email",
                table: "EntidadesAcesso",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EntidadesAcesso_Email",
                table: "EntidadesAcesso");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadesAcesso_Chave",
                table: "EntidadesAcesso",
                column: "Chave");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadesAcesso_Email",
                table: "EntidadesAcesso",
                column: "Email");
        }
    }
}
