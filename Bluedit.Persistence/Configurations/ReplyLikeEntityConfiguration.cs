using Bluedit.Domain.Entities.LikeEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bluedit.Persistence.Configurations;

internal sealed class ReplyLikeEntityConfiguration : IEntityTypeConfiguration<ReplyLike>
{
    public void Configure(EntityTypeBuilder<ReplyLike> entityBuilder)
    {
        entityBuilder.HasKey(rl => new { rl.UserId, rl.ParentId });
        entityBuilder.HasOne(rl => rl.Reply).WithMany(r => r.ReplyLikes).HasForeignKey(rl => rl.ParentId);
    }
}