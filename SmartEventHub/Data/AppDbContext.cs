using Microsoft.EntityFrameworkCore;
using SmartEventHub.Entities;
using System.Reflection.Emit;

namespace SmartEventHub.Data
{
    // AppDbContext inherits from DbContext (The Entity Framework Core base class)
    public class AppDbContext : DbContext
    {
        // ==========================================
        // 1. Constructor
        // ==========================================
        // Passes the database connection options from Program.cs to the base DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ==========================================
        // 2. DbSets (Tables)
        // ==========================================
        // Each DbSet represents a table in our SQL database
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // ==========================================
        // 3. Fluent API (Database Configuration)
        // ==========================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Always call the base method first
            base.OnModelCreating(modelBuilder);

            // Configure Registration to Session relationship (Cascade delete)
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Session)
                .WithMany(s => s.Registrations)
                .HasForeignKey(r => r.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Session to Event relationship (Cascade delete)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Event)
                .WithMany(e => e.Sessions)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================================================
            // PREVENTING MULTIPLE CASCADE PATHS ERRORS (The Fix!)
            // =======================================================

            // 1. Registration to User
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Session to Speaker (User)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Speaker)
                .WithMany(u => u.SpeakerSessions)
                .HasForeignKey(s => s.SpeakerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Event to Organizer (User)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(u => u.OrganizedEvents)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}