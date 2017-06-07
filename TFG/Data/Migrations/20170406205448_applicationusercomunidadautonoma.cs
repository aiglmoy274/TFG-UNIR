using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TFG.Data.Migrations
{
    public partial class applicationusercomunidadautonoma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ComunidadAutonoma_ComunidadAutonomaId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ComunidadAutonomaId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ComunidadAutonoma_ComunidadAutonomaId",
                table: "AspNetUsers",
                column: "ComunidadAutonomaId",
                principalTable: "ComunidadAutonoma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ComunidadAutonoma_ComunidadAutonomaId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ComunidadAutonomaId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ComunidadAutonoma_ComunidadAutonomaId",
                table: "AspNetUsers",
                column: "ComunidadAutonomaId",
                principalTable: "ComunidadAutonoma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
