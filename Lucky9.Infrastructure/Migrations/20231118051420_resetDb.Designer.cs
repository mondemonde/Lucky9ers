﻿// <auto-generated />
using System;
using Lucky9.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lucky9.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231118051420_resetDb")]
    partial class resetDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Money")
                        .HasColumnType("REAL");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Lucky9.Domain.Entities.Bet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("BetValue")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerCards")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Bets", (string)null);
                });

            modelBuilder.Entity("Lucky9.Domain.Entities.Card", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rank")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Suit")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Lucky9.Domain.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServerCards")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Games", (string)null);
                });

            modelBuilder.Entity("Lucky9.Domain.Entities.Bet", b =>
                {
                    b.HasOne("Lucky9.Domain.Entities.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });
#pragma warning restore 612, 618
        }
    }
}