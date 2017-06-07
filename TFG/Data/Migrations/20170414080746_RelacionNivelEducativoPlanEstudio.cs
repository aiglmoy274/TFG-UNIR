using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFG.Data.Migrations
{
    public partial class RelacionNivelEducativoPlanEstudio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<int>(
                name: "NivelEducativoId",
                table: "PlanEstudio",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlanEstudio_NivelEducativoId",
                table: "PlanEstudio",
                column: "NivelEducativoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanEstudio_NivelEducativo_NivelEducativoId",
                table: "PlanEstudio",
                column: "NivelEducativoId",
                principalTable: "NivelEducativo",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction
                );*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.DropForeignKey(
                name: "FK_PlanEstudio_NivelEducativo_NivelEducativoId",
                table: "PlanEstudio");

            migrationBuilder.DropIndex(
                name: "IX_PlanEstudio_NivelEducativoId",
                table: "PlanEstudio");

            migrationBuilder.DropColumn(
                name: "NivelEducativoId",
                table: "PlanEstudio");*/
        }
    }
}
