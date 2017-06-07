using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFG.Data.Migrations
{
    public partial class PlanificacionGrupo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanificacionId",
                table: "Grupo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Grupo_PlanificacionId",
                table: "Grupo",
                column: "PlanificacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupo_Planificacion_PlanificacionId",
                table: "Grupo",
                column: "PlanificacionId",
                principalTable: "Planificacion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupo_Planificacion_PlanificacionId",
                table: "Grupo");

            migrationBuilder.DropIndex(
                name: "IX_Grupo_PlanificacionId",
                table: "Grupo");

            migrationBuilder.DropColumn(
                name: "PlanificacionId",
                table: "Grupo");
        }
    }
}
