using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ConsultEaseDAL.Entities;
using ConsultEaseDAL.Entities.Auth;

namespace ConsultEaseDAL.Context
{
    public class ConsultEaseDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ConsultEaseDbContext()
        {
            
        }
        public ConsultEaseDbContext(DbContextOptions<ConsultEaseDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<CounsellingCategory> CounsellingCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Professor)
                .WithMany()
                .HasForeignKey(a => a.ProfessorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.CounsellingCategory)
                .WithMany(cc => cc!.Appointments)
                .HasForeignKey(a => a.CounsellingCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CounsellingCategory>()
                .HasMany(cc => cc.Appointments)
                .WithOne(a => a.CounsellingCategory)
                .HasForeignKey(a => a.CounsellingCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=DANKOVPC;Database=ConsultEaseDb;Trusted_Connection=True;" +
                    "trustServerCertificate=true; MultipleActiveResultSets=true");
            }
        }
    }
}