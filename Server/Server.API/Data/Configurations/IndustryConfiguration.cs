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

        WithData(builder);
    }

    protected void WithData(EntityTypeBuilder<Industry> builder)
    {
        builder.HasData(
            new Industry { Id = 1, Name = "Manufacturing" },
            new Industry { Id = 2, Name = "IT Services" },
            new Industry { Id = 3, Name = "Healthcare" },
            new Industry { Id = 4, Name = "Finance" },
            new Industry { Id = 5, Name = "Retail" },
            new Industry { Id = 6, Name = "Logistics" },
            new Industry { Id = 7, Name = "Education" },
            new Industry { Id = 8, Name = "Energy" },
            new Industry { Id = 9, Name = "Automotive" },
            new Industry { Id = 10, Name = "Food & Beverage" }
        );
    }
}