using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Entities;
using Task = SelfGrind.Domain.Entities.Task;

namespace SelfGrind.Infrastructure.Persistence;

public class SelfGrindDbContext(DbContextOptions<SelfGrindDbContext> options) : DbContext(options)
{
    internal DbSet<Task> Tasks { get; set; }
    internal DbSet<TaskSource> TaskSources { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TaskSource>()
            .HasMany(t => t.Tasks)
            .WithOne(t => t.TaskSource)
            .HasForeignKey(t => t.TaskSourceId);
    }
}