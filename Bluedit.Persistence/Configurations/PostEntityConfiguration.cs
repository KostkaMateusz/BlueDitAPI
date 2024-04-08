using Bluedit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bluedit.Persistence.Configurations;

internal sealed class PostEntityConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> entityBuilder)
    {
        entityBuilder.HasOne(post => post.User).WithMany(user => user.Posts);
        entityBuilder.HasOne(post => post.Topic).WithMany(topic => topic.Posts).HasForeignKey(post => post.TopicName);
        entityBuilder.Property(c => c.CreationDate).HasDefaultValueSql("getutcdate()");
        entityBuilder.Property(c => c.UpdateDate).ValueGeneratedOnUpdate();
    }
}