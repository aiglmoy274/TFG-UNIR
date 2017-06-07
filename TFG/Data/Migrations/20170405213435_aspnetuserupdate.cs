using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TFG.Data.Migrations
{
    public partial class aspnetuserupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComunidadAutonomaId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreCentro",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComunidadAutonoma",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComunidadAutonoma", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ComunidadAutonomaId",
                table: "AspNetUsers",
                column: "ComunidadAutonomaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ComunidadAutonoma_ComunidadAutonomaId",
                table: "AspNetUsers",
                column: "ComunidadAutonomaId",
                principalTable: "ComunidadAutonoma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ComunidadAutonoma_ComunidadAutonomaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ComunidadAutonoma");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ComunidadAutonomaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ComunidadAutonomaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NombreCentro",
                table: "AspNetUsers");
        }
    }
}
