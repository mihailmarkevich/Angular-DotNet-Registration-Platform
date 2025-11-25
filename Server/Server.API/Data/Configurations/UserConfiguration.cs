using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.API.Models;

namespace Server.API.Data.Configurations;

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
    }
}