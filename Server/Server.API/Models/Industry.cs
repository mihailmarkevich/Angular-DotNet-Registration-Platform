namespace Server.API.Models;

public class Industry
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<Company> Companies { get; set; } = new List<Company>();
}
