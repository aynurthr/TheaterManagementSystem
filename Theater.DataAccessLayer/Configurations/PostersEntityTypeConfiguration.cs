using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class PosterTypeConfiguration : IEntityTypeConfiguration<Poster>
{
    public void Configure(EntityTypeBuilder<Poster> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Genre).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Age).IsRequired().HasMaxLength(10);
        builder.Property(p => p.Duration).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(1000);
        builder.Property(p => p.ImageSrc).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.Rating).IsRequired().HasColumnType("float");

        builder.HasOne(p => p.Hall)
            .WithMany(h => h.Posters)
            .HasForeignKey(p => p.HallId);

        builder.HasMany(p => p.Roles)
            .WithOne(r => r.Poster)
            .HasForeignKey(r => r.PosterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ShowDates)
            .WithOne(sd => sd.Poster)
            .HasForeignKey(sd => sd.PosterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Poster)
            .HasForeignKey(c => c.PosterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Posters");
    }
}
