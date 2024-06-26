using Theater.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Theater.DataAccessLayer.Configurations.Membership
{
    class AppUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<AppUserLogin>
    {
        public void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            builder.Property(m => m.UserId).HasColumnType("int").IsRequired();
            builder.Property(m => m.LoginProvider).HasColumnType("nvarchar").HasMaxLength(450);
            builder.Property(m => m.ProviderKey).HasColumnType("nvarchar").HasMaxLength(450);
            builder.Property(m => m.ProviderDisplayName).HasColumnType("nvarchar(max)");

            builder.HasKey(m => new { m.LoginProvider, m.ProviderKey });
            builder.ToTable("UserLogins", "Membership");

            builder.HasOne<AppUser>()
                .WithMany()
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
