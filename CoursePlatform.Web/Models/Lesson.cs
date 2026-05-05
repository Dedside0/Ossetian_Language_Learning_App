namespace Adam.Models;

public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TheoryHtml { get; set; } = string.Empty;
    public List<Card> Cards { get; set; } = new();
    public List<ConjugationQuestion> ConjugationQuestions { get; set; } = new();
}
