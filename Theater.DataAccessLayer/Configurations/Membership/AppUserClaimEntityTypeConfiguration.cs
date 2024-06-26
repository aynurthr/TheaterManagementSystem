using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.DataAccessLayer.Configurations.Membership
{
    class AppUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<AppUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            builder.Property(m => m.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(m => m.UserId).HasColumnType("int").IsRequired();
            builder.Property(m => m.ClaimType).HasColumnType("varchar").HasMaxLength(200).IsRequired();
            builder.Property(m => m.ClaimValue).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();

            builder.HasKey(m => m.Id);
            builder.ToTable("UserClaims", "Membership");

            builder.HasOne<AppUser>()
                .WithMany()
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
