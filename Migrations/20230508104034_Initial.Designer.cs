﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sales;

#nullable disable

namespace Sales.Migrations
{
    [DbContext(typeof(SalesDbContext))]
    [Migration("20230508104034_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Sales.DTOs.ProductDto", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Sales.DTOs.ProductLineDto", b =>
                {
                    b.Property<int>("ProdLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SalesDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("SalesManId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("REAL");

                    b.HasKey("ProdLineId");

                    b.ToTable("ProductLines");
                });

            modelBuilder.Entity("Sales.DTOs.SalesManDto", b =>
                {
                    b.Property<int>("SalesManId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmployeeStat")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SalesManId");

                    b.ToTable("SalesMen");
                });
#pragma warning restore 612, 618
        }
    }
}
