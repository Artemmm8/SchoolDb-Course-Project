﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolDbProject.Models;

namespace SchoolDbProject.Migrations
{
    [DbContext(typeof(SchoolDbContext))]
    [Migration("20210511075607_Add Date Column to Mark")]
    partial class AddDateColumntoMark
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SchoolDbProject.Models.Cabinet", b =>
                {
                    b.Property<int>("CabinetId")
                        .HasColumnType("int");

                    b.Property<byte?>("NumberOfSeats")
                        .IsRequired()
                        .HasColumnType("tinyint");

                    b.HasKey("CabinetId");

                    b.ToTable("Cabinet");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("varchar(3)");

                    b.Property<byte?>("NumberOfStudents")
                        .IsRequired()
                        .HasColumnType("tinyint");

                    b.HasKey("ClassId");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Mark", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<byte?>("Mark1")
                        .HasColumnType("tinyint")
                        .HasColumnName("Mark");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Mark");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(225)
                        .IsUnicode(false)
                        .HasColumnType("varchar(225)");

                    b.Property<string>("LastName")
                        .HasMaxLength(225)
                        .IsUnicode(false)
                        .HasColumnType("varchar(225)");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)");

                    b.HasKey("StudentId");

                    b.HasIndex("ClassId");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("SchoolDbProject.Models.StudentSchedule", b =>
                {
                    b.Property<int?>("CabinetId")
                        .HasColumnType("int");

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<byte?>("DayOfWeek")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("LessonNumber")
                        .HasColumnType("tinyint");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("int");

                    b.HasIndex("CabinetId");

                    b.HasIndex("ClassId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("StudentSchedule");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("SubjectId");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(225)
                        .IsUnicode(false)
                        .HasColumnType("varchar(225)");

                    b.Property<string>("LastName")
                        .HasMaxLength(225)
                        .IsUnicode(false)
                        .HasColumnType("varchar(225)");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)");

                    b.HasKey("TeacherId");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Mark", b =>
                {
                    b.HasOne("SchoolDbProject.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK__Mark__StudentId__2C3393D0");

                    b.HasOne("SchoolDbProject.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("FK__Mark__SubjectId__3E52440B");

                    b.Navigation("Student");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Student", b =>
                {
                    b.HasOne("SchoolDbProject.Models.Class", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassId")
                        .HasConstraintName("FK__Student__ClassId__2A4B4B5E");

                    b.Navigation("Class");
                });

            modelBuilder.Entity("SchoolDbProject.Models.StudentSchedule", b =>
                {
                    b.HasOne("SchoolDbProject.Models.Cabinet", "Cabinet")
                        .WithMany()
                        .HasForeignKey("CabinetId")
                        .HasConstraintName("FK__StudentSc__Cabin__30F848ED");

                    b.HasOne("SchoolDbProject.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .HasConstraintName("FK__StudentSc__Teach__300424B4");

                    b.HasOne("SchoolDbProject.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("FK__StudentSc__Subje__31EC6D26");

                    b.HasOne("SchoolDbProject.Models.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("FK__StudentSc__Teach__32E0915F");

                    b.Navigation("Cabinet");

                    b.Navigation("Class");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SchoolDbProject.Models.Class", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
