using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;

public class CommentTypeConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CommentText).IsRequired().HasMaxLength(500);
        builder.Property(c => c.Time).IsRequired();

        builder.HasOne(c => c.Poster)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PosterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Comments");
    }
}
