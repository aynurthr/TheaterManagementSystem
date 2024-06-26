using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class HallEntityTypeConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Name).IsRequired().HasMaxLength(100);

        // Configure the SeatsPerRowJson property
        builder.Property(h => h.SeatsPerRowJson)
            .HasColumnName("SeatsPerRowJson")
            .HasColumnType("nvarchar(max)");

        builder.HasMany(h => h.Seats)
            .WithOne(s => s.Hall)
            .HasForeignKey(s => s.HallId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Halls");
    }
}
