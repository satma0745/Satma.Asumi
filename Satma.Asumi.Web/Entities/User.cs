namespace Satma.Asumi.Web.Entities;

public class User
{
    public required Guid Id { get; init; }
    public required string DisplayName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required UserRole Role { get; init; }
}

public enum UserRole
{
    Customer,
    Manager,
    Admin
}