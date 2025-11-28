namespace Server.API.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public int IndustryId { get; set; }
    public Industry Industry { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; } = new List<User>();
}
