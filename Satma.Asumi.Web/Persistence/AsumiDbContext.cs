using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Satma.Asumi.Web.Persistence.Entities;

namespace Satma.Asumi.Web.Persistence;

public class AsumiDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    
    public AsumiDbContext(DbContextOptions<AsumiDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(currentAssembly);
    }
}