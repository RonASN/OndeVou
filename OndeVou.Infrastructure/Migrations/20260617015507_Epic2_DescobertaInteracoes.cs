using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OndeVou.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Epic2_DescobertaInteracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "estabelecimentos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nota = table.Column<int>(type: "integer", nullable: false),
                    Comentario = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    EstabelecimentoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_avaliacoes_estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "estabelecimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_avaliacoes_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "favoritos",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    EstabelecimentoId = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favoritos", x => new { x.UsuarioId, x.EstabelecimentoId });
                    table.ForeignKey(
                        name: "FK_favoritos_estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "estabelecimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_favoritos_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_estabelecimentos_Categoria",
                table: "estabelecimentos",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_estabelecimentos_DataCriacao",
                table: "estabelecimentos",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_estabelecimentos_Localizacao",
                table: "estabelecimentos",
                column: "Localizacao")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_estabelecimentos_Nome",
                table: "estabelecimentos",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_avaliacoes_EstabelecimentoId",
                table: "avaliacoes",
                column: "EstabelecimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_avaliacoes_UsuarioId_EstabelecimentoId",
                table: "avaliacoes",
                columns: new[] { "UsuarioId", "EstabelecimentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_favoritos_EstabelecimentoId",
                table: "favoritos",
                column: "EstabelecimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "avaliacoes");

            migrationBuilder.DropTable(
                name: "favoritos");

            migrationBuilder.DropIndex(
                name: "IX_estabelecimentos_Categoria",
                table: "estabelecimentos");

            migrationBuilder.DropIndex(
                name: "IX_estabelecimentos_DataCriacao",
                table: "estabelecimentos");

            migrationBuilder.DropIndex(
                name: "IX_estabelecimentos_Localizacao",
                table: "estabelecimentos");

            migrationBuilder.DropIndex(
                name: "IX_estabelecimentos_Nome",
                table: "estabelecimentos");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "estabelecimentos");
        }
    }
}
