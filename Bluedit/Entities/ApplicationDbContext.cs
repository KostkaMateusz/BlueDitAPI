using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Bluedit.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<ReplyBase> Replies { get; set; }
    public DbSet<Topic> Topics { get; set; }

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
            entityBuilder.HasOne(post => post.Topic).WithMany(topic => topic.Posts).HasForeignKey(post=>post.TopicName);
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
            entityBuilder.HasDiscriminator<bool>(r => r.IsPostReplay).HasValue<Reply>(true).HasValue<SubReplay>(false);

        });            

    }
}
