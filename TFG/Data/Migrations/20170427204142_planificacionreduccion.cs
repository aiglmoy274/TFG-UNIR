using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TFG.Data.Migrations
{
    public partial class planificacionreduccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanificacionReduccion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false),
                    HorasSemanales = table.Column<int>(nullable: false),
                    PlanificacionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanificacionReduccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanificacionReduccion_Planificacion_PlanificacionId",
                        column: x => x.PlanificacionId,
                        principalTable: "Planificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanificacionReduccion_PlanificacionId",
                table: "PlanificacionReduccion",
                column: "PlanificacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanificacionReduccion");
        }
    }
}
