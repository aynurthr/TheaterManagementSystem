using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

namespace Theater.DataAccessLayer.Configurations
{
    public class SeatReservationEntityTypeConfiguration : IEntityTypeConfiguration<SeatReservation>
    {
        public void Configure(EntityTypeBuilder<SeatReservation> builder)
        {
            builder.HasKey(sr => sr.Id);
            builder.Property(sr => sr.Email).IsRequired();

            builder.HasOne(sr => sr.Seat)
                   .WithMany(s => s.SeatReservations)
                   .HasForeignKey(sr => sr.SeatId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sr => sr.ShowDate)
                   .WithMany(sd => sd.SeatReservations)
                   .HasForeignKey(sr => sr.ShowDateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("SeatReservations");
        }
    }
}
