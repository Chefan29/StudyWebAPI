using StudyWebAPI.Application.DTOs;

namespace StudyWebAPI.Application.Interfaces
{
    public interface IRemarkQueryService
    {
        Task <List<RemarkDto>> GetAllAsync();
        Task<(bool ok, string? error, RemarkDto? remarkDto)> GetByIdAsync (int id);
        
        Task <(bool ok, string? error, List<RemarkDto>? remarkDtos)> SearchAsync(string query);
    }
}
