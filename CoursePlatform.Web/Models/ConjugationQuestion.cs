namespace Adam.Models;

public class ConjugationQuestion
{
    public int Id { get; set; }
    public string RussianSentence { get; set; } = string.Empty;
    public string OssetianAnswer { get; set; } = string.Empty;
    public string? Hint { get; set; }
    public string? ImageUrl { get; set; }
}
