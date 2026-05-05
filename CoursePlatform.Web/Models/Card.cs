namespace Adam.Models;

public class Card
{
    public int Id { get; set; }
    public string OssetianWord { get; set; } = string.Empty;
    public string RussianWord { get; set; } = string.Empty;
    public string? VerbType { get; set; }
    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
}
