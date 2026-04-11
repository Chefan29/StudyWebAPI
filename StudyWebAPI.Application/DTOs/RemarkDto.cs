namespace StudyWebAPI.Application.DTOs
{
    public record RemarkDto(
        int Id,
        string Title,
        string RemarkContent,
        DateTime CreatedAt,
        bool HasImage
    );
}
