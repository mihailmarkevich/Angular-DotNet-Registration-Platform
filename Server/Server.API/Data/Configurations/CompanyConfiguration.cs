using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.API.Models;

namespace Server.API.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id)
               .HasName("PK_Companies");

        builder.Property(c => c.Id)
               .UseIdentityColumn();

        builder.Property(c => c.Name)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(c => c.CreatedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(c => c.Industry)
               .WithMany(i => i.Companies)
               .HasForeignKey(c => c.IndustryId)
               .HasConstraintName("FK_Companies_Industries")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.IndustryId)
               .HasDatabaseName("IX_Companies_IndustryId");

        builder.HasIndex(c => c.Name)
               .HasDatabaseName("IX_Companies_Name");
    }
}