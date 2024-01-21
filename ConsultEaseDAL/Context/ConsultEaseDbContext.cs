using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ConsultEaseDAL.Entities;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseDAL.Entities.Enums;


namespace ConsultEaseDAL.Context;

public class ConsultEaseDbContext: IdentityDbContext<User, IdentityRole<int>, int>
{
    public ConsultEaseDbContext(DbContextOptions options)
        : base(options)
    {
    }
    
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<CounsellingCategory> CounsellingCategories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Professor)
            .WithMany(p => p!.Appointments)
            .HasForeignKey(a => a.ProfessorId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Student)
            .WithMany(s => s!.Appointments)
            .HasForeignKey(a => a.StudentId)
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
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Appointments)
            .WithOne(a => a.Student)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Appointments)
            .WithOne(a => a.Professor)
            .HasForeignKey(a => a.ProfessorId)
            .OnDelete(DeleteBehavior.NoAction);
       
    }
}