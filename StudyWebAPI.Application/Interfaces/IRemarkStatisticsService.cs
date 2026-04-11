using StudyWebAPI.Application.DTOs;

namespace StudyWebAPI.Application.Interfaces
{
    public interface IRemarkStatisticsService
    {
        Task<StatisticsDto> GetStatisticsAsync();
    }
}
