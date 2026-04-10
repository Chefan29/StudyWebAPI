
using Microsoft.EntityFrameworkCore;
using StudyWebAPI.Application.DTOs;
using StudyWebAPI.Application.Interfaces;
using StudyWebAPI.Domain.Entities;
using StudyWebAPI.Infrastructure.Data;


namespace StudyWebAPI.Infrastructure.Services
{
    public class EfRemarkService : IRemarkQueryService
    {
        private readonly RemarkDbContext _dbContext;
        public EfRemarkService(RemarkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(bool ok, string? error, RemarkDto? remarkDto)> CreateAsync(CreateUpdateDto createUpdateDto)
        {
            if (string.IsNullOrWhiteSpace(createUpdateDto.Title))
            {
                return (false, "Заголовок замечания пуст", null);
            }
            if (string.IsNullOrWhiteSpace(createUpdateDto.RemarkContent))
            {
                return (false, "Контент замечания пуст", null);
            }

            var remark = new RemarkEntity
            {
                Title = createUpdateDto.Title,
                RemarkContent = createUpdateDto.RemarkContent,
                HasImage = createUpdateDto.HasImage,
                CreatedAt = DateTime.UtcNow,
            };

            _dbContext.Remarks.Add(remark);
            await _dbContext.SaveChangesAsync();

            var remarkDto = new RemarkDto
            (
                remark.Id,
                createUpdateDto.Title,
                createUpdateDto.RemarkContent,
                remark.CreatedAt,
                createUpdateDto.HasImage
                
            );
            return (true, null, remarkDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingRemark = await _dbContext.Remarks.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRemark == null)
                return false;

            _dbContext.Remove(existingRemark);
            await _dbContext.SaveChangesAsync();

            return true;

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


        public async Task<(bool ok, string? error, RemarkDto? remarkDto)> UpdateAsync(int id, CreateUpdateDto createUpdateDto)
        {
            var existingRemark = await _dbContext.Remarks.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRemark == null)
                return (false, "Редактируемого замечания с таким Id не существует!", null);

            if (string.IsNullOrWhiteSpace(createUpdateDto.Title))
            {
                return (false, "Заголовок замечания пуст", null);
            }

            if (string.IsNullOrWhiteSpace(createUpdateDto.RemarkContent))
            {
                return (false, "Контент замечания пуст", null);
            }

            existingRemark.Title = createUpdateDto.Title;
            existingRemark.RemarkContent = createUpdateDto.RemarkContent;
            existingRemark.HasImage = createUpdateDto.HasImage;

            await _dbContext.SaveChangesAsync();

            var remarkDto = new RemarkDto(
                existingRemark.Id,
                existingRemark.Title,
                existingRemark.RemarkContent,
                existingRemark.CreatedAt,
                existingRemark.HasImage
            );

            return (true, null, remarkDto);
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
