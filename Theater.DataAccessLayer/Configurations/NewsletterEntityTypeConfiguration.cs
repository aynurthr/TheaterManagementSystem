using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class NewsletterEntityTypeConfiguration : IEntityTypeConfiguration<Newsletter>
    {
        public void Configure(EntityTypeBuilder<Newsletter> builder)
        {
            builder.HasKey(ns => ns.Id);

            builder.Property(ns => ns.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ns => ns.IsSubscribed)
                .IsRequired();

            builder.Property(ns => ns.SubscribedAt)
                .IsRequired();

            builder.Property(ns => ns.LastModified)
                .IsRequired(false);

            builder.ToTable("Newsletters");
        }
    }
