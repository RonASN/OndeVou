using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OndeVou.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoUsuarioERelacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoUsuario",
                table: "usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "estabelecimentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_estabelecimentos_UsuarioId",
                table: "estabelecimentos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_estabelecimentos_usuarios_UsuarioId",
                table: "estabelecimentos",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estabelecimentos_usuarios_UsuarioId",
                table: "estabelecimentos");

            migrationBuilder.DropIndex(
                name: "IX_estabelecimentos_UsuarioId",
                table: "estabelecimentos");

            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "estabelecimentos");
        }
    }
}
