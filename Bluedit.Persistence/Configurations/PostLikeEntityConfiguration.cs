using Bluedit.Domain.Entities.LikeEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bluedit.Persistence.Configurations;

internal sealed class PostLikeEntityConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> entityBuilder)
    {
        entityBuilder.HasKey(rl => new { rl.UserId, rl.ParentId });
        entityBuilder.HasOne(rl => rl.Post).WithMany(r => r.PostLikes).HasForeignKey(rl => rl.ParentId);
    }
}