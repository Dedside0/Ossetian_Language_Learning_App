namespace Adam.Models;

public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string TheoryHtml { get; set; } = "";

    public List<ListeningTask> ListeningTasks { get; set; } = [];
    public List<Card> Cards { get; set; } = [];
    public List<ConjugationQuestion> ConjugationQuestions { get; set; } = [];
}
