﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Context;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Server.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Server.Models.Component", b =>
                {
                    b.Property<int>("ComponentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ComponentID"));

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("DatasheetLink")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ImageURI")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LongDescription")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ComponentID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("Server.Models.OwnsComponent", b =>
                {
                    b.Property<int>("OwnsComponentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OwnsComponentID"));

                    b.Property<int>("ComponentID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("OwnsComponentID");

                    b.HasIndex("ComponentID");

                    b.HasIndex("UserID");

                    b.ToTable("OwnsComponent");
                });

            modelBuilder.Entity("Server.Models.PredefinedImage", b =>
                {
                    b.Property<int>("PredefinedImageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PredefinedImageID"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PredefinedImageID");

                    b.ToTable("PredefinedImage");
                });

            modelBuilder.Entity("Server.Models.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ImageURI")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ProjectID");

                    b.HasIndex("UserID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Server.Models.ProjectComponent", b =>
                {
                    b.Property<int>("ProjectComponentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectComponentID"));

                    b.Property<int>("ComponentID")
                        .HasColumnType("int");

                    b.Property<int>("ProjectID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ProjectComponentID");

                    b.HasIndex("ComponentID");

                    b.HasIndex("ProjectID");

                    b.ToTable("ProjectComponents");
                });

            modelBuilder.Entity("Server.Models.Purchase", b =>
                {
                    b.Property<int>("PurchaseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PurchaseID"));

                    b.Property<DateTime>("PurchasedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SupplierID")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PurchaseID");

                    b.HasIndex("SupplierID");

                    b.HasIndex("UserID");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Server.Models.PurchaseItem", b =>
                {
                    b.Property<int>("PurchaseItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PurchaseItemID"));

                    b.Property<int>("ComponentID")
                        .HasColumnType("int");

                    b.Property<double>("PricePerUnit")
                        .HasColumnType("float");

                    b.Property<int>("PurchaseID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("PurchaseItemID");

                    b.HasIndex("ComponentID");

                    b.HasIndex("PurchaseID");

                    b.ToTable("PurchaseItems");
                });

            modelBuilder.Entity("Server.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SupplierID"));

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("SupplierID");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Server.Models.Component", b =>
                {
                    b.HasOne("Server.Models.Category", "Category")
                        .WithMany("Components")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Server.Models.OwnsComponent", b =>
                {
                    b.HasOne("Server.Models.Component", "Component")
                        .WithMany("OwnsComponents")
                        .HasForeignKey("ComponentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.User", "User")
                        .WithMany("OwnsComponents")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Component");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.Project", b =>
                {
                    b.HasOne("Server.Models.User", "User")
                        .WithMany("Projects")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.ProjectComponent", b =>
                {
                    b.HasOne("Server.Models.Component", "Component")
                        .WithMany("ProjectComponents")
                        .HasForeignKey("ComponentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Project", "Project")
                        .WithMany("ProjectComponents")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Component");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Server.Models.Purchase", b =>
                {
                    b.HasOne("Server.Models.Supplier", "Supplier")
                        .WithMany("Purchases")
                        .HasForeignKey("SupplierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.PurchaseItem", b =>
                {
                    b.HasOne("Server.Models.Component", "Component")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("ComponentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Purchase", "Purchase")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("PurchaseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Component");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("Server.Models.Category", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("Server.Models.Component", b =>
                {
                    b.Navigation("OwnsComponents");

                    b.Navigation("ProjectComponents");

                    b.Navigation("PurchaseItems");
                });

            modelBuilder.Entity("Server.Models.Project", b =>
                {
                    b.Navigation("ProjectComponents");
                });

            modelBuilder.Entity("Server.Models.Purchase", b =>
                {
                    b.Navigation("PurchaseItems");
                });

            modelBuilder.Entity("Server.Models.Supplier", b =>
                {
                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Navigation("OwnsComponents");

                    b.Navigation("Projects");

                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
