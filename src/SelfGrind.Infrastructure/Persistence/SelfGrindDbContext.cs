using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Infrastructure.Persistence;

public class SelfGrindDbContext(DbContextOptions<SelfGrindDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskOccurrence> TaskOccurrences { get; set; }
    public DbSet<TaskSchedule> TaskSchedules { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitEntry> HabitEntries { get; set; }
    public DbSet<UserStat> UserStats { get; set; }
    public DbSet<EvolutionTier> EvolutionTiers { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<UserItem> UserItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
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

                task.HasOne(t => t.User)
                    .WithMany(u => u.TaskItems)
                    .HasForeignKey(t => t.UserId);

                task.HasIndex(t => new { t.UserId, t.IsArchived });
            });

        modelBuilder.Entity<TaskOccurrence>(occurrence =>
        {
            occurrence.HasIndex(o => new { o.Status, o.ScheduledDate });
            occurrence.HasIndex(o => new { o.Status, o.CompletedDate });
        });

        modelBuilder.Entity<Habit>(habit =>
        {
            habit.HasOne(h => h.User)
                .WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId);
            
            habit.HasMany(h => h.HabitEntries)
                .WithOne(e => e.Habit)
                .HasForeignKey(e => e.HabitId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HabitEntry>(entry =>
        {
            entry.HasIndex(e => new { e.HabitId, e.EntryDate });
        });
        
        modelBuilder.Entity<UserStat>(userStat =>
        {
            userStat.HasOne(s => s.User)
                .WithMany(u => u.Stats)
                .HasForeignKey(s => s.UserId);
        });

        modelBuilder.Entity<EvolutionTier>(tier =>
        {
            tier.Property(t => t.Name).HasMaxLength(64);
            tier.Property(t => t.Emoji).HasMaxLength(16);
            tier.Property(t => t.StageName).HasMaxLength(64);
            tier.Property(t => t.NextEvolutionLabel).HasMaxLength(128);
            tier.HasIndex(t => t.Order).IsUnique();

            tier.HasData(
                new EvolutionTier { Id = Guid.Parse("11111111-1111-1111-1111-000000000001"), Order = 1, Name = "Novice", Emoji = "🌱", MinLevel = 1, MaxLevel = 10, StageName = "Novice Adventurer", NextEvolutionLabel = "Level 11: Apprentice" },
                new EvolutionTier { Id = Guid.Parse("11111111-1111-1111-1111-000000000002"), Order = 2, Name = "Apprentice", Emoji = "📖", MinLevel = 11, MaxLevel = 20, StageName = "Apprentice", NextEvolutionLabel = "Level 21: Adept" },
                new EvolutionTier { Id = Guid.Parse("11111111-1111-1111-1111-000000000003"), Order = 3, Name = "Adept", Emoji = "⚡", MinLevel = 21, MaxLevel = 30, StageName = "Adept", NextEvolutionLabel = "Level 31: Expert" },
                new EvolutionTier { Id = Guid.Parse("11111111-1111-1111-1111-000000000004"), Order = 4, Name = "Expert", Emoji = "🔥", MinLevel = 31, MaxLevel = 40, StageName = "Elite Warrior", NextEvolutionLabel = "Level 41: Master" },
                new EvolutionTier { Id = Guid.Parse("11111111-1111-1111-1111-000000000005"), Order = 5, Name = "Master", Emoji = "👑", MinLevel = 41, MaxLevel = 50, StageName = "Legendary Champion", NextEvolutionLabel = null }
            );
        });

        modelBuilder.Entity<Skill>(skill =>
        {
            skill.Property(s => s.Name).HasMaxLength(64);
            skill.Property(s => s.Emoji).HasMaxLength(16);
            skill.Property(s => s.Description).HasMaxLength(256);
            skill.Property(s => s.Attribute).HasConversion<string>().HasMaxLength(32);
            skill.HasIndex(s => new { s.Attribute, s.Order }).IsUnique();

            skill.HasData(
                // Strength
                new Skill { Id = Guid.Parse("22222222-0000-0000-0001-000000000001"), Name = "Warrior I", Emoji = "⚔️", Description = "Reach Strength level 1", Attribute = Domain.Constants.BaseAttribute.Strength, RequiredAttributeLevel = 1, Order = 1 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0001-000000000002"), Name = "Warrior II", Emoji = "🗡️", Description = "Reach Strength level 5", Attribute = Domain.Constants.BaseAttribute.Strength, RequiredAttributeLevel = 5, Order = 2 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0001-000000000003"), Name = "Warrior III", Emoji = "🛡️", Description = "Reach Strength level 10", Attribute = Domain.Constants.BaseAttribute.Strength, RequiredAttributeLevel = 10, Order = 3 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0001-000000000004"), Name = "Titan", Emoji = "💥", Description = "Reach Strength level 20", Attribute = Domain.Constants.BaseAttribute.Strength, RequiredAttributeLevel = 20, Order = 4 },
                // Knowledge
                new Skill { Id = Guid.Parse("22222222-0000-0000-0002-000000000001"), Name = "Scholar I", Emoji = "📖", Description = "Reach Knowledge level 1", Attribute = Domain.Constants.BaseAttribute.Knowledge, RequiredAttributeLevel = 1, Order = 1 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0002-000000000002"), Name = "Scholar II", Emoji = "📚", Description = "Reach Knowledge level 5", Attribute = Domain.Constants.BaseAttribute.Knowledge, RequiredAttributeLevel = 5, Order = 2 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0002-000000000003"), Name = "Scholar III", Emoji = "🎓", Description = "Reach Knowledge level 10", Attribute = Domain.Constants.BaseAttribute.Knowledge, RequiredAttributeLevel = 10, Order = 3 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0002-000000000004"), Name = "Sage", Emoji = "🧙", Description = "Reach Knowledge level 20", Attribute = Domain.Constants.BaseAttribute.Knowledge, RequiredAttributeLevel = 20, Order = 4 },
                // Focus
                new Skill { Id = Guid.Parse("22222222-0000-0000-0003-000000000001"), Name = "Monk I", Emoji = "🧘", Description = "Reach Focus level 1", Attribute = Domain.Constants.BaseAttribute.Focus, RequiredAttributeLevel = 1, Order = 1 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0003-000000000002"), Name = "Monk II", Emoji = "🌿", Description = "Reach Focus level 5", Attribute = Domain.Constants.BaseAttribute.Focus, RequiredAttributeLevel = 5, Order = 2 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0003-000000000003"), Name = "Monk III", Emoji = "☯️", Description = "Reach Focus level 10", Attribute = Domain.Constants.BaseAttribute.Focus, RequiredAttributeLevel = 10, Order = 3 },
                new Skill { Id = Guid.Parse("22222222-0000-0000-0003-000000000004"), Name = "Zen Master", Emoji = "✨", Description = "Reach Focus level 20", Attribute = Domain.Constants.BaseAttribute.Focus, RequiredAttributeLevel = 20, Order = 4 }
            );
        });

        modelBuilder.Entity<Item>(item =>
        {
            item.Property(i => i.Name).HasMaxLength(64);
            item.Property(i => i.Emoji).HasMaxLength(16);
            item.Property(i => i.Type).HasMaxLength(32);
            item.Property(i => i.Bonus).HasMaxLength(128);
            item.Property(i => i.Rarity).HasConversion<string>().HasMaxLength(16);
            item.Property(i => i.Variant).HasConversion<string>().HasMaxLength(16);

            item.HasData(
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000001"), Name = "Health Potion", Emoji = "🧪", Type = "Consumable", Bonus = "+10% Energy", Rarity = Domain.Constants.ItemRarity.Common, Variant = Domain.Constants.ItemVariant.Success, UnlockLevel = 1 },
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000002"), Name = "Wisdom Scroll", Emoji = "📜", Type = "Artifact", Bonus = "+15 Knowledge XP", Rarity = Domain.Constants.ItemRarity.Uncommon, Variant = Domain.Constants.ItemVariant.Info, UnlockLevel = 1 },
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000003"), Name = "Focus Amulet", Emoji = "📿", Type = "Artifact", Bonus = "+25 Focus XP", Rarity = Domain.Constants.ItemRarity.Rare, Variant = Domain.Constants.ItemVariant.Violet, UnlockLevel = 3 },
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000004"), Name = "Power Band", Emoji = "💎", Type = "Accessory", Bonus = "+20% Strength", Rarity = Domain.Constants.ItemRarity.Epic, Variant = Domain.Constants.ItemVariant.Error, UnlockLevel = 5 },
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000005"), Name = "Power Gauntlets", Emoji = "🧤", Type = "Gloves", Bonus = "+20 Strength", Rarity = Domain.Constants.ItemRarity.Epic, Variant = Domain.Constants.ItemVariant.Error, UnlockLevel = 5 },
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000006"), Name = "Life Amulet", Emoji = "💚", Type = "Artifact", Bonus = "+30 Vitality", Rarity = Domain.Constants.ItemRarity.Rare, Variant = Domain.Constants.ItemVariant.Success, UnlockLevel = 4 },
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000007"), Name = "XP Booster", Emoji = "⭐", Type = "Boost", Bonus = "+50% XP Gain", Rarity = Domain.Constants.ItemRarity.Legendary, Variant = Domain.Constants.ItemVariant.Warning, UnlockLevel = 10 },
                new Item { Id = Guid.Parse("33333333-0000-0000-0000-000000000008"), Name = "Energy Crystal", Emoji = "⚡", Type = "Consumable", Bonus = "+22 Energy", Rarity = Domain.Constants.ItemRarity.Uncommon, Variant = Domain.Constants.ItemVariant.Warning, UnlockLevel = 2 }
            );
        });

        modelBuilder.Entity<UserItem>(userItem =>
        {
            userItem.HasOne(ui => ui.User)
                .WithMany()
                .HasForeignKey(ui => ui.UserId);

            userItem.HasOne(ui => ui.Item)
                .WithMany()
                .HasForeignKey(ui => ui.ItemId);

            userItem.HasIndex(ui => new { ui.UserId, ui.ItemId }).IsUnique();
        });

        modelBuilder.Entity<RefreshToken>(token =>
        {
            token.Property(t => t.TokenHash).HasMaxLength(64);
            token.HasIndex(t => t.TokenHash).IsUnique();

            token.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}