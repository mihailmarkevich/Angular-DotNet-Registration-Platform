using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.API.Models;

namespace Server.API.Data.Configurations;

public class IndustryConfiguration : IEntityTypeConfiguration<Industry>
{
    public void Configure(EntityTypeBuilder<Industry> builder)
    {
        builder.ToTable("Industries");

        builder.HasKey(i => i.Id)
               .HasName("PK_Industries");

        builder.Property(i => i.Id)
               .UseIdentityColumn();

        builder.Property(i => i.Name)
               .HasMaxLength(200)
               .IsRequired();

    }
}