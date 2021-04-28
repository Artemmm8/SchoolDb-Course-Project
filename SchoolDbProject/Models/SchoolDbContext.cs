using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SchoolDbProject.Models
{
    public partial class SchoolDbContext : DbContext
    {
        public SchoolDbContext()
        {
        }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cabinet> Cabinets { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentSchedule> StudentSchedules { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Cabinet>(entity =>
            {
                entity.ToTable("Cabinet");

                entity.Property(e => e.CabinetId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.ClassId).ValueGeneratedNever();

                entity.Property(e => e.ClassName)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Mark");

                entity.Property(e => e.Mark1).HasColumnName("Mark");

                entity.HasOne(d => d.Student)
                    .WithMany()
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Mark__StudentId__2C3393D0");

                entity.HasOne(d => d.Subject)
                    .WithMany()
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Mark__SubjectId__3E52440B");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.StudentId).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(225)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(225)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Student__ClassId__2A4B4B5E");
            });

            modelBuilder.Entity<StudentSchedule>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("StudentSchedule");

                entity.HasOne(d => d.Cabinet)
                    .WithMany()
                    .HasForeignKey(d => d.CabinetId)
                    .HasConstraintName("FK__StudentSc__Cabin__30F848ED");

                entity.HasOne(d => d.Class)
                    .WithMany()
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__StudentSc__Teach__300424B4");

                entity.HasOne(d => d.Subject)
                    .WithMany()
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__StudentSc__Subje__31EC6D26");

                entity.HasOne(d => d.Teacher)
                    .WithMany()
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__StudentSc__Teach__32E0915F");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.SubjectId).ValueGeneratedNever();

                entity.Property(e => e.SubjectName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.Property(e => e.TeacherId).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(225)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(225)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
