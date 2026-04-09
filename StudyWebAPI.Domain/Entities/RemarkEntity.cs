namespace StudyWebAPI.Domain.Entities
{
    public class RemarkEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string RemarkContent { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public bool HasImage { get; set; }
    }
}
