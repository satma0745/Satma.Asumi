namespace Satma.Asumi.Web.Components.Pages;

public partial class Users
{
    private class UserViewModel
    {
        public required int Index { get; init; }
        public required string DisplayName { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Email { get; init; }
        public required UserRole Role { get; init; }
    }

    private enum UserRole
    {
        Customer,
        Manager,
        Admin
    }

    private IReadOnlyCollection<UserViewModel> GetUsers(int pageNumber, int pageSize)
    {
        var startingIndex = pageSize * pageNumber;
        return Enumerable
            .Range(startingIndex, pageSize)
            .Select(index => new UserViewModel
            {
                Index = index + 1,
                DisplayName = "Stanislav Lebedyantsev",
                PhoneNumber = "+375 (29) 367-85-14",
                Email = "stas.lebed@gmail.com",
                Role = UserRole.Customer
            })
            .ToList();
    }
}