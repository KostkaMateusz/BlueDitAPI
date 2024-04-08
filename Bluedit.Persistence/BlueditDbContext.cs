using System.Reflection;
using Bluedit.Domain.Entities;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Domain.Entities.ReplyEntities;
using Microsoft.EntityFrameworkCore;

namespace Bluedit.Persistence;

public class BlueditDbContext : DbContext
{
    public BlueditDbContext(DbContextOptions<BlueditDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<ReplyBase> Replies { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<ReplyLike> ReplyLikes { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }

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
