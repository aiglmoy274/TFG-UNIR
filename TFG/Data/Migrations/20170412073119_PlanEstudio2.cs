using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFG.Data.Migrations
{
    public partial class PlanEstudio2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComunidadAutonomaId",
                table: "PlanEstudio",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlanEstudio_ComunidadAutonomaId",
                table: "PlanEstudio",
                column: "ComunidadAutonomaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanEstudio_ComunidadAutonoma_ComunidadAutonomaId",
                table: "PlanEstudio",
                column: "ComunidadAutonomaId",
                principalTable: "ComunidadAutonoma",
                principalColumn: "ComunidadAutonomaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanEstudio_ComunidadAutonoma_ComunidadAutonomaId",
                table: "PlanEstudio");

            migrationBuilder.DropIndex(
                name: "IX_PlanEstudio_ComunidadAutonomaId",
                table: "PlanEstudio");

            migrationBuilder.DropColumn(
                name: "ComunidadAutonomaId",
                table: "PlanEstudio");
        }
    }
}
