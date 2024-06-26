using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.DataAccessLayer.Configurations.Membership
{
    class AppRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.Property(m => m.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(m => m.Name).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(m => m.NormalizedName).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(m => m.ConcurrencyStamp).HasColumnType("varchar").HasMaxLength(100).IsRequired();

            builder.HasKey(m => m.Id);
            builder.ToTable("Roles", "Membership");
        }
    }
}
