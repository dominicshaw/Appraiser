using Appraiser.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Appraiser.Data
{
    public class AppraiserContext : DbContext
    {
        public DbSet<Appraisal> Appraisals { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Training> Training { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }

        public DbSet<Change> Changes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LONHSQL01\\PROD01;Database=Appraisals;Trusted_Connection=True;App=Appraiser");
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Appraisal>()
                .HasIndex(u => new { u.StaffId, u.PeriodStart, u.Deleted })
                .IsUnique();
        }
    }
}