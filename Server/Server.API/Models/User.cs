namespace Server.API.Models;

public class User
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!; // unique
    public string? Email { get; set; }

    public byte[] PasswordHash { get; set; } = default!;
    public byte[]? PasswordSalt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime TermsAcceptedAt { get; set; }
    public DateTime PrivacyAcceptedAt { get; set; }
}
