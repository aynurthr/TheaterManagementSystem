using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

namespace Theater.DataAccessLayer.Configurations
{
    public class ContactPostEntityTypeConfiguration : IEntityTypeConfiguration<ContactPost>
    {
        public void Configure(EntityTypeBuilder<ContactPost> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Topic).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Message).IsRequired();
            builder.Property(c => c.SentAt).IsRequired();
            builder.Property(c => c.AnsweredAt).IsRequired(false);
            builder.Property(c => c.AnsweredBy).HasMaxLength(100);

            builder.ToTable("Contacts");
        }
    }
}
