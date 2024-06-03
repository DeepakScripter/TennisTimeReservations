using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TennisCourtBookingApp.Repository.Models
{
    public partial class TestSPC5Context : DbContext
    {
        public TestSPC5Context()
        {
        }

        public TestSPC5Context(DbContextOptions<TestSPC5Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AssignTaskBug> AssignTaskBugs { get; set; } = null!;
        public virtual DbSet<AssignTaskProject> AssignTaskProjects { get; set; } = null!;
        public virtual DbSet<AssignTaskProjectAccess> AssignTaskProjectAccesses { get; set; } = null!;
        public virtual DbSet<AssignTaskRole> AssignTaskRoles { get; set; } = null!;
        public virtual DbSet<AssignTaskUser> AssignTaskUsers { get; set; } = null!;
        public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; } = null!;
        public virtual DbSet<TaskDocument> TaskDocuments { get; set; } = null!;
        public virtual DbSet<TennisCourt> TennisCourts { get; set; } = null!;
        public virtual DbSet<TennisCourtBooking> TennisCourtBookings { get; set; } = null!;
        public virtual DbSet<TennisCourtBookingRole> TennisCourtBookingRoles { get; set; } = null!;
        public virtual DbSet<TennisCourtBookingUser> TennisCourtBookingUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=TestSPC5; User Id=traininguser; Password=admin123; MultipleActiveResultSets=True; TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignTaskBug>(entity =>
            {
                entity.HasKey(e => e.TaskId);

                entity.Property(e => e.CompletedOn).HasColumnType("datetime");

                entity.Property(e => e.DocumentsRelated).IsUnicode(false);

                entity.Property(e => e.TaskDescription).IsUnicode(false);

                entity.Property(e => e.TaskType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeLimit).HasColumnType("datetime");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.AssignTaskBugAssignedToNavigations)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("FK_AssignTaskBugs_AssignTaskUser1");

                entity.HasOne(d => d.CompletedByNavigation)
                    .WithMany(p => p.AssignTaskBugCompletedByNavigations)
                    .HasForeignKey(d => d.CompletedBy)
                    .HasConstraintName("FK_AssignTaskBugs_AssignTaskUser");

                entity.HasOne(d => d.ProjectldNavigation)
                    .WithMany(p => p.AssignTaskBugs)
                    .HasForeignKey(d => d.Projectld)
                    .HasConstraintName("FK_AssignTaskBugs_AssignTaskProjects");
            });

            modelBuilder.Entity<AssignTaskProject>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ProjectDeadLine).HasColumnType("datetime");

                entity.Property(e => e.ProjectName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.ProjectLead)
                    .WithMany(p => p.AssignTaskProjects)
                    .HasForeignKey(d => d.ProjectLeadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignTaskProjects_AssignTaskUser");
            });

            modelBuilder.Entity<AssignTaskProjectAccess>(entity =>
            {
                entity.HasKey(e => e.ProjectAccessId);

                entity.ToTable("AssignTaskProjectAccess");

                entity.Property(e => e.ProjectAccessId).HasColumnName("ProjectAccessID");

                entity.HasOne(d => d.AccessToldNavigation)
                    .WithMany(p => p.AssignTaskProjectAccesses)
                    .HasForeignKey(d => d.AccessTold)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignTaskProjectAccess_AssignTaskUser");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.AssignTaskProjectAccesses)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignTaskProjectAccess_AssignTaskProjects");
            });

            modelBuilder.Entity<AssignTaskRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("AssignTaskRole");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AssignTaskUser>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.ToTable("AssignTaskUser");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AssignTaskUsers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignTaskUser_AssignTaskRole");
            });

            modelBuilder.Entity<ProjectDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.ToTable("ProjectDocument");

                entity.Property(e => e.DocumentName).IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectDocuments)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectDocument_AssignTaskProjects");
            });

            modelBuilder.Entity<TaskDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.ToTable("TaskDocument");

                entity.Property(e => e.DocumentName).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskDocuments)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskDocument_AssignTaskBugs");
            });

            modelBuilder.Entity<TennisCourt>(entity =>
            {
                entity.ToTable("TennisCourt");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.TennisCourtAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TennisCourtName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("date");
            });

            modelBuilder.Entity<TennisCourtBooking>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.ToTable("TennisCourtBooking");

                entity.Property(e => e.BookingDate).HasColumnType("date");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TennisCourtBookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_TennisCourtBooking_TennisCourtBookingUser");
            });

            modelBuilder.Entity<TennisCourtBookingRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("TennisCourtBookingRole");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TennisCourtBookingUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("TennisCourtBookingUser");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("date");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TennisCourtBookingUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_TennisCourtBookingUser_TennisCourtBookingRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
