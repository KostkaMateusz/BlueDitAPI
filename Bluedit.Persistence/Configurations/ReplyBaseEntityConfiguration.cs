using Bluedit.Domain.Entities.ReplyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bluedit.Persistence.Configurations;

internal sealed class ReplyBaseEntityConfiguration : IEntityTypeConfiguration<ReplyBase>
{
    public void Configure(EntityTypeBuilder<ReplyBase> entityBuilder)
    {
        entityBuilder.HasKey(r => r.ReplyId);
        entityBuilder.HasDiscriminator(r => r.IsPostReplay).HasValue<Reply>(true).HasValue<SubReplay>(false);
        entityBuilder.Property(r => r.CreationDate).HasDefaultValueSql("getutcdate()");
    }
}