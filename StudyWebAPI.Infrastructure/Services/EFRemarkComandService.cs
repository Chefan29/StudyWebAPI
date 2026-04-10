using Microsoft.EntityFrameworkCore;
using StudyWebAPI.Application.DTOs;
using StudyWebAPI.Application.Interfaces;
using StudyWebAPI.Domain.Entities;
using StudyWebAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWebAPI.Infrastructure.Services
{
    public class EFRemarkComandService : IRemarkComandService
    {
        private readonly RemarkDbContext _dbContext;
        public EFRemarkComandService(RemarkDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
