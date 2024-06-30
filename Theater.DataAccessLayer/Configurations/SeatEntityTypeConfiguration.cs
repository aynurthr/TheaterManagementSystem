using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class SeatTypeConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Row).IsRequired().HasMaxLength(5);
        builder.Property(s => s.Number).IsRequired();
        builder.Property(s => s.Price).IsRequired().HasColumnType("decimal(18,2)");

        builder.HasOne(s => s.Hall)
            .WithMany(h => h.Seats)
            .HasForeignKey(s => s.HallId);

        builder.ToTable("Seats");
    }
}
