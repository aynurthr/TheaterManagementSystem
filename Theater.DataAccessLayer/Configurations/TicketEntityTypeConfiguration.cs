using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class TicketTypeConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(t => t.IsPurchased).IsRequired();

        builder.HasOne(t => t.ShowDate)
            .WithMany(sd => sd.Tickets)
            .HasForeignKey(t => t.ShowDateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Seat)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.SeatId)
            .OnDelete(DeleteBehavior.Restrict); // Change to Restrict to avoid cascading delete conflict

        builder.ToTable("Tickets");
    }
}
