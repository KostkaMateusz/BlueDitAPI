using Bluedit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bluedit.Persistence.Configurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entityBuilder)
    {
        entityBuilder.Property(user => user.CreationTime).HasDefaultValueSql("getutcdate()");
        entityBuilder.HasMany(user => user.Posts).WithOne(post => post.User);
        entityBuilder.Property(user => user.Role).HasDefaultValue("StandartUser");
    }
}