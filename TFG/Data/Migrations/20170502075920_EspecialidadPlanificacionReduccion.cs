using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFG.Data.Migrations
{
    public partial class EspecialidadPlanificacionReduccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EspecialidadId",
                table: "PlanificacionReduccion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlanificacionReduccion_EspecialidadId",
                table: "PlanificacionReduccion",
                column: "EspecialidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanificacionReduccion_Especialidad_EspecialidadId",
                table: "PlanificacionReduccion",
                column: "EspecialidadId",
                principalTable: "Especialidad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanificacionReduccion_Especialidad_EspecialidadId",
                table: "PlanificacionReduccion");

            migrationBuilder.DropIndex(
                name: "IX_PlanificacionReduccion_EspecialidadId",
                table: "PlanificacionReduccion");

            migrationBuilder.DropColumn(
                name: "EspecialidadId",
                table: "PlanificacionReduccion");
        }
    }
}
