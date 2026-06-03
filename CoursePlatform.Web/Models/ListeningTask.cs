namespace Adam.Models
{
    public class ListeningTask
    {
        public int Id { get; set; }
        public string AudioUrl { get; set; }

        public string AudioDecoding { get; set; }

        public string? RussianTranslation { get; set; }
    }
}