﻿// <auto-generated />
using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CommandAPI.Migrations
{
    [DbContext(typeof(CommandContext))]
    [Migration("20220808182324_AddCommandsToDB")]
    partial class AddCommandsToDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CommandAPI.Models.Command", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CommandLine")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HowTo")
                        .IsRequired()
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CommandItems");
                });
#pragma warning restore 612, 618
        }
    }
}
