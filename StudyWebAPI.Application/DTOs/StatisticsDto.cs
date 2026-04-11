namespace StudyWebAPI.Application.DTOs
{
    public record StatisticsDto(
        int TotalRemarks,
        int RemarksWithImage,
        int RemarksWithoutImage,
        IReadOnlyCollection<TopWordDto> TopWords,
        IReadOnlyCollection<RemarksByDayDto> RemarksByDay
        );
}
