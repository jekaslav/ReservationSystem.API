﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReservationSystem.Domain.Contexts;

namespace ReservationSystem.API.Migrations
{
    [DbContext(typeof(ReservationDbContext))]
    partial class ReservationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ChiefClassroomEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChiefId")
                        .HasColumnType("int");

                    b.Property<int>("ClassroomId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChiefId");

                    b.HasIndex("ClassroomId");

                    b.ToTable("ChiefClassrooms");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ChiefEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Chiefs");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ClassroomEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Capacity")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomNumber")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ReservationEntity", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("ClassroomId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("EndTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("StudentId");

                    b.HasIndex("ClassroomId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ReservationRequestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassroomId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("EndTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("StudentId");

                    b.ToTable("ReservationRequests");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.StudentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ChiefClassroomEntity", b =>
                {
                    b.HasOne("ReservationSystem.Domain.Entities.ChiefEntity", "Chief")
                        .WithMany("ChiefClassrooms")
                        .HasForeignKey("ChiefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReservationSystem.Domain.Entities.ClassroomEntity", "Classroom")
                        .WithMany("ChiefClassrooms")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chief");

                    b.Navigation("Classroom");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ReservationEntity", b =>
                {
                    b.HasOne("ReservationSystem.Domain.Entities.ClassroomEntity", "Classroom")
                        .WithMany("ReservationList")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReservationSystem.Domain.Entities.StudentEntity", "Student")
                        .WithMany("ReservationList")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classroom");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ReservationRequestEntity", b =>
                {
                    b.HasOne("ReservationSystem.Domain.Entities.ClassroomEntity", "Classroom")
                        .WithMany("ReservationRequestList")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReservationSystem.Domain.Entities.StudentEntity", "Student")
                        .WithMany("ReservationRequestList")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classroom");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ChiefEntity", b =>
                {
                    b.Navigation("ChiefClassrooms");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.ClassroomEntity", b =>
                {
                    b.Navigation("ChiefClassrooms");

                    b.Navigation("ReservationList");

                    b.Navigation("ReservationRequestList");
                });

            modelBuilder.Entity("ReservationSystem.Domain.Entities.StudentEntity", b =>
                {
                    b.Navigation("ReservationList");

                    b.Navigation("ReservationRequestList");
                });
#pragma warning restore 612, 618
        }
    }
}
