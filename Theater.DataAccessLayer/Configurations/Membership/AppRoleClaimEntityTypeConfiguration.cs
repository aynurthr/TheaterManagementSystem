using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.DataAccessLayer.Configurations.Membership
{
    class AppRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<AppRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {
            builder.Property(m => m.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(m => m.RoleId).HasColumnType("int").IsRequired();
            builder.Property(m => m.ClaimType).HasColumnType("varchar").HasMaxLength(200).IsRequired();
            builder.Property(m => m.ClaimValue).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();

            builder.HasKey(m => m.Id);
            builder.ToTable("RoleClaims", "Membership");

            builder.HasOne<AppRole>()
                .WithMany()
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(m => m.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
