using Microsoft.EntityFrameworkCore;

namespace Bluedit.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entityBuilder =>
        {
            entityBuilder.Property(user => user.CreationTime).HasDefaultValueSql("getutcdate()");
            entityBuilder.HasMany(user => user.Posts).WithOne(post => post.User); 
        });

        modelBuilder.Entity<Post>(entityBuilder =>
        {
            entityBuilder.HasOne(post => post.User).WithMany(user => user.Posts);
            entityBuilder.Property(c => c.UpdateDate).ValueGeneratedOnUpdate();
        });
    }
}
