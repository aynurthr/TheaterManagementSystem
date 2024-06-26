using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class ActorTypeConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FullName).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Title).HasMaxLength(100);
        builder.Property(a => a.ImageSrc).HasMaxLength(255);

        builder.HasMany(a => a.Roles)
            .WithOne(r => r.Actor)
            .HasForeignKey(r => r.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Actors");
    }
}
