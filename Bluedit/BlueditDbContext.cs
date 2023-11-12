using Microsoft.EntityFrameworkCore;
using Bluedit.Domain.Entities;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit;

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
        modelBuilder.Entity<User>(entityBuilder =>
        {
            entityBuilder.Property(user => user.CreationTime).HasDefaultValueSql("getutcdate()");
            entityBuilder.HasMany(user => user.Posts).WithOne(post => post.User);
            entityBuilder.Property(user => user.Role).HasDefaultValue("StandartUser");
        });

        modelBuilder.Entity<Post>(entityBuilder =>
        {
            entityBuilder.HasOne(post => post.User).WithMany(user => user.Posts);
            entityBuilder.HasOne(post => post.Topic).WithMany(topic => topic.Posts).HasForeignKey(post => post.TopicName);
            entityBuilder.Property(c => c.CreationDate).HasDefaultValueSql("getutcdate()");
            entityBuilder.Property(c => c.UpdateDate).ValueGeneratedOnUpdate();
        });

        modelBuilder.Entity<Topic>(entityBuilder =>
        {
            entityBuilder.HasKey(topic => topic.TopicName);
            entityBuilder.HasMany(topic => topic.Posts).WithOne(post => post.Topic);

        });

        modelBuilder.Entity<ReplyBase>(entityBuilder =>
        {
            entityBuilder.HasKey(r => r.ReplyId);
            entityBuilder.HasDiscriminator(r => r.IsPostReplay).HasValue<Reply>(true).HasValue<SubReplay>(false);

        });

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
