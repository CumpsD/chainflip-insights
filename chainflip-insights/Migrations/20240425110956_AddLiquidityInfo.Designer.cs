﻿// <auto-generated />
using System;
using ChainflipInsights.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChainflipInsights.Migrations
{
    [DbContext(typeof(BotContext))]
    [Migration("20240425110956_AddLiquidityInfo")]
    partial class AddLiquidityInfo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ChainflipInsights.EntityFramework.BurnInfo", b =>
                {
                    b.Property<ulong>("BurnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("BurnId"));

                    b.Property<double>("BurnAmount")
                        .HasColumnType("double");

                    b.Property<uint>("BurnBlock")
                        .HasColumnType("int unsigned");

                    b.Property<DateTimeOffset>("BurnDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("BurnHash")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("varchar(120)");

                    b.Property<string>("ExplorerUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("BurnId");

                    b.HasIndex("BurnDate");

                    b.ToTable("burn_info", (string)null);
                });

            modelBuilder.Entity("ChainflipInsights.EntityFramework.LiquidityInfo", b =>
                {
                    b.Property<ulong>("LiquidityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("LiquidityId"));

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<double>("AmountUsd")
                        .HasColumnType("double");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("ExplorerUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("LiquidityDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("LiquidityId");

                    b.HasIndex("Asset");

                    b.HasIndex("LiquidityDate");

                    b.ToTable("liquidity_info", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}