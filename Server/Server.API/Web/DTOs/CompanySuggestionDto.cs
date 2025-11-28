namespace Server.API.Web.DTOs
{
    public class CompanySuggestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int IndustryId { get; set; }
    }

}
