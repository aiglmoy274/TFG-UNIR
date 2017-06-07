using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFG.Data.Migrations
{
    public partial class RelAsignaturaEspecialidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EspecialidadId",
                table: "Asignatura",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Asignatura_EspecialidadId",
                table: "Asignatura",
                column: "EspecialidadId");

            
            migrationBuilder.AddForeignKey(
                name: "FK_Asignatura_Especialidad_EspecialidadId",
                table: "Asignatura",
                column: "EspecialidadId",
                principalTable: "Especialidad",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
                
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignatura_Especialidad_EspecialidadId",
                table: "Asignatura");

            migrationBuilder.DropIndex(
                name: "IX_Asignatura_EspecialidadId",
                table: "Asignatura");

            migrationBuilder.DropColumn(
                name: "EspecialidadId",
                table: "Asignatura");
        }
    }
}
