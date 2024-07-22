using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutenticacaoDoisFatores.Infra.Migrations
{
    /// <inheritdoc />
    public partial class flagativoentidadeacesso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "EntidadesAcesso",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "EntidadesAcesso");
        }
    }
}
