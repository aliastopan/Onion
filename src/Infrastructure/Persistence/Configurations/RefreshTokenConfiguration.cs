using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Domain.Entities.Identity;

namespace Onion.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("dbo.refresh_token");

        builder.Property(x => x.Id)
               .HasColumnName("token_id")
               .HasMaxLength(36)
               .IsRequired();

        builder.Property(x => x.Token)
               .HasColumnName("token")
               .IsRequired();

        builder.Property(x => x.JwtId)
               .HasColumnName("jwt_id")
               .IsRequired();

        builder.Property(x => x.CreationDate)
               .HasColumnName("creation_date")
               .IsRequired();

        builder.Property(x => x.ExpiryDate)
               .HasColumnName("expiry_date")
               .IsRequired();

        builder.Property(x => x.IsUsed)
               .HasColumnName("is_used")
               .IsRequired();

        builder.Property(x => x.IsInvalidated)
               .HasColumnName("is_invalidated")
               .IsRequired();

        builder.Property(x => x.UserId)
               .HasColumnName("user_id")
               .IsRequired();

        builder.HasOne(x => x.User)
               .WithMany()
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
