
using Microsoft.EntityFrameworkCore;
using StudyWebAPI.Application.DTOs;
using StudyWebAPI.Application.Interfaces;
using StudyWebAPI.Domain.Entities;
using StudyWebAPI.Infrastructure.Data;


namespace StudyWebAPI.Infrastructure.Services
{
    public class EfRemarkQueryService : IRemarkQueryService
    {
        private readonly RemarkDbContext _dbContext;
        public EfRemarkQueryService(RemarkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<RemarkDto>> GetAllAsync()
        {
           return await _dbContext.Remarks
                .AsNoTracking()
                .OrderBy(r => r.Id)
                .Select(r => new RemarkDto
                (r.Id,
                r.Title,
                r.RemarkContent,
                r.CreatedAt,
                r.HasImage
                    ))
                .ToListAsync();
        }

        public async Task<(bool ok, string? error, RemarkDto? remarkDto)> GetByIdAsync(int id)
        {
            var existingRemark = await _dbContext.Remarks.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRemark == null)
                return (false, "Замечания с таким Id не существует!", null);

            var remarks =  await _dbContext.Remarks
             .AsNoTracking()
             .Where(r => r.Id == id)
             .Select(r => new RemarkDto
             (r.Id,
             r.Title,
             r.RemarkContent,
             r.CreatedAt,
             r.HasImage
                 ))
             .FirstOrDefaultAsync();

            return (true, null, remarks);
        }

        public async Task<(bool ok, string? error, List<RemarkDto>? remarkDtos)> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return (false, "Строка поиска пустая", (List<RemarkDto>?)null);
            }
            query = query.Trim(); //!!!
            var Remarks = await _dbContext.Remarks
                .AsNoTracking()
                .Where(r => EF.Functions.ToTsVector("russian", r.Title)
                .Matches(EF.Functions.PlainToTsQuery("russian", query)))
                .OrderBy(r => r.Id)
                .Select(r => new RemarkDto(
                    r.Id,
                    r.Title,
                    r.RemarkContent,
                    r.CreatedAt,
                    r.HasImage
                ))
                .ToListAsync();
            return (true, null, Remarks);
        }
    }
}
