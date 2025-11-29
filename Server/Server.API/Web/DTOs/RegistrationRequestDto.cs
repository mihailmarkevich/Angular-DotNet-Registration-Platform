using System.ComponentModel.DataAnnotations;

namespace Server.API.Web.DTOs;

public class RegistrationRequestDto
{
    [Required]
    [StringLength(255)]
    [RegularExpression(@"^[^<>]*$", ErrorMessage = "Company name contains invalid characters.")]
    public string CompanyName { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Industry is required.")]
    public int IndustryId { get; set; }

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[^<>]*$", ErrorMessage = "First name contains invalid characters.")]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[^<>]*$", ErrorMessage = "Last name contains invalid characters.")]
    public string LastName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username contains invalid characters.")]
    public string UserName { get; set; } = null!;

    [EmailAddress]
    [StringLength(256)]
    public string? Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string PasswordRepeat { get; set; } = null!;

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Terms must be accepted.")]
    public bool AcceptTerms { get; set; }

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Privacy policy must be accepted.")]
    public bool AcceptPrivacy { get; set; }
}

