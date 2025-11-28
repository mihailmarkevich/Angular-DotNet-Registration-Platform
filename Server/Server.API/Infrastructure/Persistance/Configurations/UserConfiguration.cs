using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.API.Domain.Entities;

namespace Server.API.Infrastructure.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id)
               .HasName("PK_Users");

        builder.Property(u => u.Id)
               .UseIdentityColumn();

        builder.Property(u => u.FirstName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.LastName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.UserName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.Email)
               .HasMaxLength(256);

        builder.Property(u => u.PasswordHash)
               .IsRequired();

        builder.Property(u => u.PasswordSalt);

        builder.Property(u => u.CreatedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(u => u.TermsAcceptedAt)
               .HasColumnType("datetime2(3)")
               .IsRequired();

        builder.Property(u => u.PrivacyAcceptedAt)
               .HasColumnType("datetime2(3)")
               .IsRequired();

        builder.HasOne(u => u.Company)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.CompanyId)
               .HasConstraintName("FK_Users_Companies")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(u => u.UserName)
               .IsUnique()
               .HasDatabaseName("IX_Users_UserName");

        builder.HasIndex(u => u.CompanyId)
               .HasDatabaseName("IX_Users_CompanyId");

        WithData(builder);
    }

    protected void WithData(EntityTypeBuilder<User> builder)
    {
        var acceptDate = new DateTime(2024, 2, 1, 12, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new User
            {
                Id = 1,
                CompanyId = 1, // Alpha Manufacturing
                FirstName = "Michael",
                LastName = "Keller",
                UserName = "michael.keller",
                Email = "michael.keller@example.com",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5 }, // placeholder
                PasswordSalt = new byte[] { 9, 8, 7, 6 },     // placeholder
                CreatedAt = new DateTime(2024, 2, 1, 12, 0, 0, DateTimeKind.Utc),
                TermsAcceptedAt = acceptDate,
                PrivacyAcceptedAt = acceptDate
            },
            new User
            {
                Id = 2,
                CompanyId = 1, // SAME COMPANY as user 1
                FirstName = "Sarah",
                LastName = "Brandt",
                UserName = "sarah.brandt",
                Email = "sarah.brandt@example.com",
                PasswordHash = new byte[] { 1, 1, 2, 2, 3 },
                PasswordSalt = new byte[] { 4, 4, 5, 5 },
                CreatedAt = new DateTime(2024, 2, 2, 12, 0, 0, DateTimeKind.Utc),
                TermsAcceptedAt = acceptDate,
                PrivacyAcceptedAt = acceptDate
            },
            new User
            {
                Id = 3,
                CompanyId = 2, // Beta IT
                FirstName = "Thomas",
                LastName = "Fischer",
                UserName = "thomas.fischer",
                Email = "thomas.fischer@example.com",
                PasswordHash = new byte[] { 5, 4, 3, 2, 1 },
                PasswordSalt = new byte[] { 1, 2, 3, 4 },
                CreatedAt = new DateTime(2024, 2, 3, 12, 0, 0, DateTimeKind.Utc),
                TermsAcceptedAt = acceptDate,
                PrivacyAcceptedAt = acceptDate
            },
            new User
            {
                Id = 4,
                CompanyId = 3, // MediCare
                FirstName = "Laura",
                LastName = "Schmidt",
                UserName = "laura.schmidt",
                Email = "laura.schmidt@example.com",
                PasswordHash = new byte[] { 9, 9, 8, 8, 7 },
                PasswordSalt = new byte[] { 3, 3, 2, 2 },
                CreatedAt = new DateTime(2024, 2, 4, 12, 0, 0, DateTimeKind.Utc),
                TermsAcceptedAt = acceptDate,
                PrivacyAcceptedAt = acceptDate
            },
            new User
            {
                Id = 5,
                CompanyId = 4, // SecureFinance
                FirstName = "Jonas",
                LastName = "Müller",
                UserName = "jonas.mueller",
                Email = "jonas.mueller@example.com",
                PasswordHash = new byte[] { 7, 7, 7, 7, 7 },
                PasswordSalt = new byte[] { 1, 3, 5, 7 },
                CreatedAt = new DateTime(2024, 2, 5, 12, 0, 0, DateTimeKind.Utc),
                TermsAcceptedAt = acceptDate,
                PrivacyAcceptedAt = acceptDate
            }
        );

    }

}