using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Satma.Asumi.Web.Persistence.Entities;

public class User
{
    public required Guid Id { get; init; }
    
    public required string DisplayName { get; init; }
    public required string PhoneNumber { get; init; }
    
    public required string Email { get; init; }
    public required string Password { get; set; }
    
    public required UserRole Role { get; init; }
    // TODO: Drop the `Utc` postfix.
    public required DateTime RegisteredAtUtc { get; init; }
    
    public List<UserSession>? Sessions { get; init; }
}

public enum UserRole
{
    Customer,
    Manager,
    Admin
}

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userEntity)
    {
        userEntity.ToTable("users");

        userEntity
            .HasKey(user => user.Id)
            .HasName("pk_users");

        userEntity
            .Property(user => user.Id)
            .HasColumnName("id");

        userEntity
            .Property(user => user.DisplayName)
            .HasColumnName("display_name");

        userEntity
            .Property(user => user.PhoneNumber)
            .HasColumnName("phone_number");

        userEntity
            .Property(user => user.Email)
            .HasColumnName("email");

        userEntity
            .Property(user => user.Password)
            .HasColumnName("password");

        userEntity
            .Property(user => user.Role)
            .HasColumnName("role")
            .HasConversion<string>();

        userEntity
            .Property(user => user.RegisteredAtUtc)
            .HasColumnName("registered_at_utc");
    }
}
