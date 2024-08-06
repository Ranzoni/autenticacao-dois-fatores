﻿// <auto-generated />
using System;
using AutenticacaoDoisFatores.Infra.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutenticacaoDoisFatores.Infra.Migrations
{
    [DbContext(typeof(AutenticacaoDoisFatoresContexto))]
    partial class AutenticacaoDoisFatoresContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AutenticacaoDoisFatores.Core.Entidades.EntidadeAcesso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("boolean");

                    b.Property<string>("Chave")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DataUltimoAcesso")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("EntidadesAcesso");
                });

            modelBuilder.Entity("AutenticacaoDoisFatores.Core.Entidades.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<bool>("Ativo")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DataUltimoAcesso")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("AutenticacaoDoisFatores.Core.Entidades.Usuario", b =>
                {
                    b.HasOne("AutenticacaoDoisFatores.Core.Entidades.EntidadeAcesso", "EntidadeAcesso")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EntidadeAcesso");
                });
#pragma warning restore 612, 618
        }
    }
}
