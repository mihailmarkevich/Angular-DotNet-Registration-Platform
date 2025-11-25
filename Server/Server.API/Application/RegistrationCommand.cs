namespace Server.API.Application
{
    public sealed class RegistrationCommand
    {
        public string CompanyName { get; init; } = null!;
        public int IndustryId { get; init; }

        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string UserName { get; init; } = null!;

        public string? Email { get; init; }

        public string Password { get; init; } = null!;

        public bool AcceptTerms { get; init; }
        public bool AcceptPrivacy { get; init; }
    }

    public sealed class RegistrationResult
    {
        public int CompanyId { get; init; }
        public int UserId { get; init; }
    }

}
