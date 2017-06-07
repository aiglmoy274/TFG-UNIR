using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TFG.Data.Migrations
{
    public partial class asignatura1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Normativa",
                table: "PlanEstudio",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "Asignatura",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CursoEscolarId = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false),
                    HorasSemanales = table.Column<int>(nullable: false),
                    PlanEstudioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asignatura_CursoEscolar_CursoEscolarId",
                        column: x => x.CursoEscolarId,
                        principalTable: "CursoEscolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asignatura_PlanEstudio_PlanEstudioId",
                        column: x => x.PlanEstudioId,
                        principalTable: "PlanEstudio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asignatura_CursoEscolarId",
                table: "Asignatura",
                column: "CursoEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_Asignatura_PlanEstudioId",
                table: "Asignatura",
                column: "PlanEstudioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignatura");

            migrationBuilder.AlterColumn<string>(
                name: "Normativa",
                table: "PlanEstudio",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
