using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class ShowDateTypeConfiguration : IEntityTypeConfiguration<ShowDate>
{
    public void Configure(EntityTypeBuilder<ShowDate> builder)
    {
        builder.HasKey(sd => sd.Id);
        builder.Property(sd => sd.Date).IsRequired();
        builder.Property(sd => sd.Time).IsRequired();

        builder.HasOne(sd => sd.Poster)
            .WithMany(p => p.ShowDates)
            .HasForeignKey(sd => sd.PosterId);

        builder.HasOne(sd => sd.Hall)
            .WithMany(h => h.ShowDates)
            .HasForeignKey(sd => sd.HallId);

        builder.ToTable("ShowDates");
    }
}
