using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

namespace Theater.DataAccessLayer.Configurations
{
    public class SeatEntityTypeConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Row).IsRequired();
            builder.Property(s => s.Number).IsRequired();
            builder.Property(s => s.Price).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne(s => s.Hall)
                   .WithMany(h => h.Seats)
                   .HasForeignKey(s => s.HallId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ConfigureAuditable();
            builder.ToTable("Seats");
        }
    }
}
