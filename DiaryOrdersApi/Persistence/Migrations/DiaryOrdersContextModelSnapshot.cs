﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrdersApi.Persistence;

#nullable disable

namespace OrdersApi.Persistence.Migrations
{
    [DbContext(typeof(DiaryOrdersContext))]
    partial class DiaryOrdersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.1.23111.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrdersApi.Models.DiaryOrder", b =>
                {
                    b.Property<Guid>("DiaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentItem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FeelingScore")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DiaryId");

                    b.ToTable("DiaryOrders");
                });

            modelBuilder.Entity("OrdersApi.Models.DiaryOrderDetail", b =>
                {
                    b.Property<Guid>("DiaryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GeneratedDiaryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("GeneratedContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DiaryId", "GeneratedDiaryId");

                    b.ToTable("DiaryOrderDetails");
                });

            modelBuilder.Entity("OrdersApi.Models.DiaryOrderDetail", b =>
                {
                    b.HasOne("OrdersApi.Models.DiaryOrder", null)
                        .WithMany("DiaryOrderDetails")
                        .HasForeignKey("DiaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrdersApi.Models.DiaryOrder", b =>
                {
                    b.Navigation("DiaryOrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
