using DevJobs.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.API.Persistence;
public class DevJobsContext : DbContext
{
    public DevJobsContext(DbContextOptions<DevJobsContext> options) : base(options)
    {
    }
    
    public DbSet<JobVacancy> JobVacancies {get; set; }
    public DbSet<JobApplication> JobApplications {get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobVacancy>(x => {
            x.HasKey(jv => jv.Id);
            // x.ToTable("JobVacancies");

            x.HasMany(jv => jv.Applications)
                .WithOne()
                .HasForeignKey(a => a.IdJobVacancy)
                .OnDelete(DeleteBehavior.Restrict);            
        });

        modelBuilder.Entity<JobApplication>(x => {
            x.HasKey(jb => jb.Id);
            // x.ToTable("JobApplications");
        });
    }
}