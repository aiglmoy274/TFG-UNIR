using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TFG.Data.Migrations
{
    public partial class PlanificacionAsignatura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CursoEscolarId = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grupo_CursoEscolar_CursoEscolarId",
                        column: x => x.CursoEscolarId,
                        principalTable: "CursoEscolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanificacionAsignatura",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AsignaturaId = table.Column<int>(nullable: false),
                    EspecialidadId = table.Column<int>(nullable: false),
                    GrupoId = table.Column<int>(nullable: false),
                    HorasSemanales = table.Column<int>(nullable: false),
                    PlanificacionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanificacionAsignatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanificacionAsignatura_Asignatura_AsignaturaId",
                        column: x => x.AsignaturaId,
                        principalTable: "Asignatura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanificacionAsignatura_Especialidad_EspecialidadId",
                        column: x => x.EspecialidadId,
                        principalTable: "Especialidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanificacionAsignatura_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanificacionAsignatura_Planificacion_PlanificacionId",
                        column: x => x.PlanificacionId,
                        principalTable: "Planificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grupo_CursoEscolarId",
                table: "Grupo",
                column: "CursoEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanificacionAsignatura_AsignaturaId",
                table: "PlanificacionAsignatura",
                column: "AsignaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanificacionAsignatura_EspecialidadId",
                table: "PlanificacionAsignatura",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanificacionAsignatura_GrupoId",
                table: "PlanificacionAsignatura",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanificacionAsignatura_PlanificacionId",
                table: "PlanificacionAsignatura",
                column: "PlanificacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanificacionAsignatura");

            migrationBuilder.DropTable(
                name: "Grupo");
        }
    }
}
