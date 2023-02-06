﻿// <auto-generated />
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations.EfDb
{
    [DbContext(typeof(EfDbContext))]
    [Migration("20230206013836_CreateEfDb")]
    partial class CreateEfDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.EntityFramework.Menu", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthorizeRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MenuName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Menu", (string)null);

                    b.HasData(
                        new
                        {
                            ID = "01042040-543f-4520-b07c-fb66da39db59",
                            AuthorizeRole = "accounting",
                            Content = "The Accounting Department handles financial transactions and record-keeping. They provide financial analysis and ensure compliance with laws and ethics. The team is crucial to the financial stability of the organization.",
                            MenuName = "accounting Department",
                            Title = "Accounting Department: Ensuring Safe and Accurate Management of Financial Transactions"
                        },
                        new
                        {
                            ID = "008a6b4a-7c9e-4f19-b1ea-30c2f39ef5d4",
                            AuthorizeRole = "software",
                            Content = "The Software Department creates and maintains software for innovation and efficiency. They use latest technology and deliver high-quality products. The team is essential for the success and growth of the company.",
                            MenuName = "software Department",
                            Title = "Software Department: Driving Innovation and Efficiency through Technology"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
