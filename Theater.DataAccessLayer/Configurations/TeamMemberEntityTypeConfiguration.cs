using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Models.Entities;


namespace Theater.DataAccessLayer.Configurations
{ 
    public class TeamMemberEntityTypeConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Role).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Biography).IsRequired();
            builder.Property(t => t.ImageUrl).IsRequired().HasMaxLength(200);

            builder.ToTable("TeamMembers");
            builder.ConfigureAuditable();
            builder.HasKey(m => m.Id);
        }
    }
}
