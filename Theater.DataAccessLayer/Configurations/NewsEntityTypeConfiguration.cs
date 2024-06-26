using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

namespace Theater.DataAccessLayer.Configurations
{
    public class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                   .HasColumnType("int")
                   .UseIdentityColumn(1, 1);

            builder.Property(n => n.Title)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(n => n.Date)
                   .HasColumnType("datetime")
                   .IsRequired();

            builder.Property(n => n.Description)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired();

            builder.Property(n => n.ImageSrc)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(500);

            builder.ConfigureAuditable();
            builder.HasKey(m => m.Id);
            builder.ToTable("News");
        }
    }
}
