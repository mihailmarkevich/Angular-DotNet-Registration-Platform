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

        WithData(builder);
    }

    protected void WithData(EntityTypeBuilder<Company> builder)
    {
        builder.HasData(
            new Company
            {
                Id = 1,
                Name = "Alpha Manufacturing GmbH",
                IndustryId = 1,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 2,
                Name = "Beta IT Solutions AG",
                IndustryId = 2,
                CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 3,
                Name = "MediCare Kliniken GmbH",
                IndustryId = 3,
                CreatedAt = new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 4,
                Name = "SecureFinance AG",
                IndustryId = 4,
                CreatedAt = new DateTime(2024, 1, 4, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 5,
                Name = "RetailX Handels GmbH",
                IndustryId = 5,
                CreatedAt = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 6,
                Name = "LogiTrans Logistics GmbH",
                IndustryId = 6,
                CreatedAt = new DateTime(2024, 1, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 7,
                Name = "EduTech Academy GmbH",
                IndustryId = 7,
                CreatedAt = new DateTime(2024, 1, 7, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 8,
                Name = "GreenEnergy Solutions AG",
                IndustryId = 8,
                CreatedAt = new DateTime(2024, 1, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 9,
                Name = "AutoDrive Systems GmbH",
                IndustryId = 9,
                CreatedAt = new DateTime(2024, 1, 9, 0, 0, 0, DateTimeKind.Utc)
            },
            new Company
            {
                Id = 10,
                Name = "FreshBite Foods GmbH",
                IndustryId = 10,
                CreatedAt = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}