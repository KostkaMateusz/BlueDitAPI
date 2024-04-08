using Bluedit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bluedit.Persistence.Configurations;

internal sealed class TopicEntityConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> entityBuilder)
    {
        entityBuilder.HasKey(topic => topic.TopicName);
        entityBuilder.HasMany(topic => topic.Posts).WithOne(post => post.Topic);
    }
}