namespace StudyWebAPI.Application.DTOs
{
    public record RemarksByDayDto(
        DateOnly Day,
        int Count
        );
}
