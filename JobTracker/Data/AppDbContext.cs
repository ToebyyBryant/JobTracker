using Microsoft.EntityFrameworkCore;
using JobTracker.Models;

namespace JobTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
}