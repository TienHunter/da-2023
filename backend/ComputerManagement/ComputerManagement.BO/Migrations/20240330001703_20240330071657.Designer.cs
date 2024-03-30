﻿// <auto-generated />
using System;
using ComputerManagement.BO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ComputerManagement.BO.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240330001703_20240330071657")]
    partial class _20240330071657
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ComputerManagement.BO.Models.Computer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AddressIP")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ComputerRoomId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Condition")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PositionX")
                        .HasColumnType("int");

                    b.Property<int>("PositionY")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ComputerRoomId");

                    b.ToTable("computer");
                });

            modelBuilder.Entity("ComputerManagement.BO.Models.ComputerRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CurrentCapacity")
                        .HasColumnType("int");

                    b.Property<int>("MaxCapacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("computer_room");
                });

            modelBuilder.Entity("ComputerManagement.BO.Models.ScheduleBookRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ComputerRoomId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ComputerRoomId");

                    b.HasIndex("UserId");

                    b.ToTable("schedule_book_room");
                });

            modelBuilder.Entity("ComputerManagement.BO.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("RoleIDText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("ComputerManagement.BO.Models.Computer", b =>
                {
                    b.HasOne("ComputerManagement.BO.Models.ComputerRoom", "ComputerRoom")
                        .WithMany("Computers")
                        .HasForeignKey("ComputerRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ComputerRoom");
                });

            modelBuilder.Entity("ComputerManagement.BO.Models.ScheduleBookRoom", b =>
                {
                    b.HasOne("ComputerManagement.BO.Models.ComputerRoom", "ComputerRoom")
                        .WithMany("ScheduleBookRooms")
                        .HasForeignKey("ComputerRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ComputerManagement.BO.Models.User", "User")
                        .WithMany("ScheduleBookRooms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ComputerRoom");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ComputerManagement.BO.Models.ComputerRoom", b =>
                {
                    b.Navigation("Computers");

                    b.Navigation("ScheduleBookRooms");
                });

            modelBuilder.Entity("ComputerManagement.BO.Models.User", b =>
                {
                    b.Navigation("ScheduleBookRooms");
                });
#pragma warning restore 612, 618
        }
    }
}
