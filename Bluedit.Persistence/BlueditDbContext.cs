﻿using System.Reflection;
using Bluedit.Domain.Entities;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Domain.Entities.ReplyEntities;
using Microsoft.EntityFrameworkCore;

namespace Bluedit.Persistence;

public class BlueditDbContext : DbContext
{
    public BlueditDbContext(DbContextOptions<BlueditDbContext> options) : base(options) { }

    public DbSet<User> Users { get; init; }
    public DbSet<Post> Posts { get; init; }
    public DbSet<ReplyBase> Replies { get; init; }
    public DbSet<Topic> Topics { get; init; }
    public DbSet<ReplyLike> ReplyLikes { get; init; }
    public DbSet<PostLike> PostLikes { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<ReplyLike>(entityBuilder =>
        {
            entityBuilder.HasKey(rl => new { rl.UserId, rl.ParentId });
            entityBuilder.HasOne(rl => rl.Reply).WithMany(r => r.ReplyLikes).HasForeignKey(rl => rl.ParentId);

        });

        modelBuilder.Entity<PostLike>(entityBuilder =>
        {
            entityBuilder.HasKey(rl => new { rl.UserId, rl.ParentId });
            entityBuilder.HasOne(rl => rl.Post).WithMany(r => r.PostLikes).HasForeignKey(rl => rl.ParentId);

        });
    }
}
