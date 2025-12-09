using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Infrastructure.Persistence;

public class SelfGrindDbContext(DbContextOptions<SelfGrindDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskOccurrence> TaskOccurrences { get; set; }
    public DbSet<TaskSchedule> TaskSchedules { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TaskItem>((task) =>
            {
                task.HasMany(t => t.Occurrences)
                    .WithOne(t => t.TaskItem)
                    .HasForeignKey(t => t.TaskItemId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                task.HasOne(t => t.Schedule)
                    .WithOne(s => s.TaskItem)
                    .HasForeignKey<TaskSchedule>(s => s.TaskItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

    }
}