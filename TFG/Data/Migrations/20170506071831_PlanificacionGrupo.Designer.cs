﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TFG.Data;

namespace TFG.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170506071831_PlanificacionGrupo")]
    partial class PlanificacionGrupo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TFG.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("CodigoCentro");

                    b.Property<int>("ComunidadAutonomaId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NombreCentro");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ComunidadAutonomaId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TFG.Models.Asignatura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CursoEscolarId");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("EspecialidadId");

                    b.Property<int>("HorasSemanales");

                    b.Property<int>("PlanEstudioId");

                    b.HasKey("Id");

                    b.HasIndex("CursoEscolarId");

                    b.HasIndex("EspecialidadId");

                    b.HasIndex("PlanEstudioId");

                    b.ToTable("Asignatura");
                });

            modelBuilder.Entity("TFG.Models.ComunidadAutonoma", b =>
                {
                    b.Property<int>("ComunidadAutonomaId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ComunidadAutonomaId");

                    b.ToTable("ComunidadAutonoma");
                });

            modelBuilder.Entity("TFG.Models.CursoEscolar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("NivelEducativoId");

                    b.HasKey("Id");

                    b.HasIndex("NivelEducativoId");

                    b.ToTable("CursoEscolar");
                });

            modelBuilder.Entity("TFG.Models.Especialidad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.HasKey("Id");

                    b.ToTable("Especialidad");
                });

            modelBuilder.Entity("TFG.Models.Grupo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CursoEscolarId");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("PlanificacionId");

                    b.HasKey("Id");

                    b.HasIndex("CursoEscolarId");

                    b.HasIndex("PlanificacionId");

                    b.ToTable("Grupo");
                });

            modelBuilder.Entity("TFG.Models.NivelEducativo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NivelEducativo");
                });

            modelBuilder.Entity("TFG.Models.PlanEstudio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComunidadAutonomaId");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("NivelEducativoId");

                    b.Property<string>("Normativa")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ComunidadAutonomaId");

                    b.HasIndex("NivelEducativoId");

                    b.ToTable("PlanEstudio");
                });

            modelBuilder.Entity("TFG.Models.Planificacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("HorasSemanales");

                    b.Property<string>("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Planificacion");
                });

            modelBuilder.Entity("TFG.Models.PlanificacionAsignatura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AsignaturaId");

                    b.Property<int>("EspecialidadId");

                    b.Property<int>("GrupoId");

                    b.Property<int>("HorasSemanales");

                    b.Property<int>("PlanificacionId");

                    b.HasKey("Id");

                    b.HasIndex("AsignaturaId");

                    b.HasIndex("EspecialidadId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("PlanificacionId");

                    b.ToTable("PlanificacionAsignatura");
                });

            modelBuilder.Entity("TFG.Models.PlanificacionReduccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("EspecialidadId");

                    b.Property<int>("HorasSemanales");

                    b.Property<int>("PlanificacionId");

                    b.HasKey("Id");

                    b.HasIndex("EspecialidadId");

                    b.HasIndex("PlanificacionId");

                    b.ToTable("PlanificacionReduccion");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TFG.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TFG.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TFG.Models.ApplicationUser", b =>
                {
                    b.HasOne("TFG.Models.ComunidadAutonoma", "ComunidadAutonoma")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("ComunidadAutonomaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TFG.Models.Asignatura", b =>
                {
                    b.HasOne("TFG.Models.CursoEscolar", "CursoEscolar")
                        .WithMany("Asignaturas")
                        .HasForeignKey("CursoEscolarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.Especialidad", "Especialidad")
                        .WithMany("Asignaturas")
                        .HasForeignKey("EspecialidadId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.PlanEstudio", "PlanEstudio")
                        .WithMany("Asignaturas")
                        .HasForeignKey("PlanEstudioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TFG.Models.CursoEscolar", b =>
                {
                    b.HasOne("TFG.Models.NivelEducativo", "NivelEducativo")
                        .WithMany("CursosEscolares")
                        .HasForeignKey("NivelEducativoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TFG.Models.Grupo", b =>
                {
                    b.HasOne("TFG.Models.CursoEscolar", "CursoEscolar")
                        .WithMany("Grupos")
                        .HasForeignKey("CursoEscolarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.Planificacion", "Planificacion")
                        .WithMany("Grupos")
                        .HasForeignKey("PlanificacionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TFG.Models.PlanEstudio", b =>
                {
                    b.HasOne("TFG.Models.ComunidadAutonoma", "ComunidadAutonoma")
                        .WithMany("PlanesEstudios")
                        .HasForeignKey("ComunidadAutonomaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.NivelEducativo", "NivelEducativo")
                        .WithMany("PlanesEstudio")
                        .HasForeignKey("NivelEducativoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TFG.Models.Planificacion", b =>
                {
                    b.HasOne("TFG.Models.ApplicationUser", "Usuario")
                        .WithMany("Planificaciones")
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("TFG.Models.PlanificacionAsignatura", b =>
                {
                    b.HasOne("TFG.Models.Asignatura", "Asignatura")
                        .WithMany("PlanificacionesAsignatura")
                        .HasForeignKey("AsignaturaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.Especialidad", "Especialidad")
                        .WithMany("PlanificacionesAsignatura")
                        .HasForeignKey("EspecialidadId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.Grupo", "Grupo")
                        .WithMany("PlanificacionesAsignatura")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.Planificacion", "Planificacion")
                        .WithMany("PlanificacionesAsignatura")
                        .HasForeignKey("PlanificacionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TFG.Models.PlanificacionReduccion", b =>
                {
                    b.HasOne("TFG.Models.Especialidad", "Especialidad")
                        .WithMany("PlanificacionesReduccion")
                        .HasForeignKey("EspecialidadId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TFG.Models.Planificacion", "Planificacion")
                        .WithMany("PlanificacionesReduccion")
                        .HasForeignKey("PlanificacionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
