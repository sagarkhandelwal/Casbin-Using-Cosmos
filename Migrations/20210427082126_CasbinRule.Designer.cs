﻿// <auto-generated />
using CasbinRBAC.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CasbinRBAC.Migrations
{
    [DbContext(typeof(CasbinDbContext<int>))]
    [Migration("20210427082126_CasbinRule")]
    partial class CasbinRule
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CasbinRBAC.Domain.Models.CasbinRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("V0")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("V1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("V2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("V3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("V4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("V5")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CasbinRule");
                });
#pragma warning restore 612, 618
        }
    }
}
