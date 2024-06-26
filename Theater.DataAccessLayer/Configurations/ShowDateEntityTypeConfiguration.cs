using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class ShowDateTypeConfiguration : IEntityTypeConfiguration<ShowDate>
{
    public void Configure(EntityTypeBuilder<ShowDate> builder)
    {
        builder.HasKey(sd => sd.Id);
        builder.Property(sd => sd.Date).IsRequired();

        builder.HasOne(sd => sd.Poster)
            .WithMany(p => p.ShowDates)
            .HasForeignKey(sd => sd.PosterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sd => sd.SeatReservations)
            .WithOne(sr => sr.ShowDate)
            .HasForeignKey(sr => sr.ShowDateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("ShowDates");
    }
}
