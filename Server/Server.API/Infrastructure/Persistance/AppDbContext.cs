using Microsoft.EntityFrameworkCore;
using Server.API.Domain.Entities;
using Server.API.Infrastructure.Persistance.Configurations;

namespace Server.API.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Industry> Industries => Set<Industry>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("registration");

        modelBuilder.ApplyConfiguration(new IndustryConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
