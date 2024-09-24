using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutenticacaoDoisFatores.Infra.Migrations
{
    /// <inheritdoc />
    public partial class identidadeacessousuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_EntidadesAcesso_Id",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "IdEntidadeAcesso",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdEntidadeAcesso",
                table: "Usuarios",
                column: "IdEntidadeAcesso");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_EntidadesAcesso_IdEntidadeAcesso",
                table: "Usuarios",
                column: "IdEntidadeAcesso",
                principalTable: "EntidadesAcesso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_EntidadesAcesso_IdEntidadeAcesso",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdEntidadeAcesso",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdEntidadeAcesso",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_EntidadesAcesso_Id",
                table: "Usuarios",
                column: "Id",
                principalTable: "EntidadesAcesso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
