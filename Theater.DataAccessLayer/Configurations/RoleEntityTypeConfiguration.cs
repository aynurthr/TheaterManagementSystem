using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class RoleTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.RoleName).IsRequired().HasMaxLength(50);

        builder.HasOne(r => r.Actor)
            .WithMany(a => a.Roles)
            .HasForeignKey(r => r.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Poster)
            .WithMany(p => p.Roles)
            .HasForeignKey(r => r.PosterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Roles");
    }
}
