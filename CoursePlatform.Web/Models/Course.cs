namespace Adam.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = "📚";
    public bool IsUserCreated { get; set; }
    public List<Lesson> Lessons { get; set; } = new();
}
