using Microsoft.EntityFrameworkCore;
using GestionAbsences.Models;

namespace GestionAbsences.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Role).HasConversion<string>();
            });

            // Class configuration - professor relationship
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasOne(c => c.Professor)
                    .WithMany(u => u.TaughtClasses)
                    .HasForeignKey(c => c.ProfessorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Enrollment configuration
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasIndex(e => new { e.StudentId, e.ClassId }).IsUnique();
                
                entity.HasOne(e => e.Student)
                    .WithMany(u => u.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ClassEntity)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.ClassId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Absence configuration
            modelBuilder.Entity<Absence>(entity =>
            {
                entity.HasOne(a => a.Student)
                    .WithMany(u => u.Absences)
                    .HasForeignKey(a => a.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.ClassEntity)
                    .WithMany(c => c.Absences)
                    .HasForeignKey(a => a.ClassId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.MarkedBy)
                    .WithMany()
                    .HasForeignKey(a => a.MarkedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Notification configuration
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
