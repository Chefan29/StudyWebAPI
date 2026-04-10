using Microsoft.EntityFrameworkCore;
using StudyWebAPI.Application.DTOs;
using StudyWebAPI.Application.Interfaces;
using StudyWebAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWebAPI.Infrastructure.Services
{
    public class EFRemarkStatisticsService : IRemarkStatisticsService
    {
        private readonly RemarkDbContext _dbContext;
        public EFRemarkStatisticsService(RemarkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<StatisticsDto> GetStatisticsAsync()
        {
            var topWordsTask = GetTopWordsAsync();
            var remarksByDayTask = GetRemarksByDayAsync();

            var summary = await _dbContext.Remarks
                .AsNoTracking()
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    TotalRemarks = g.Count(),
                    RemarksWithImage = g.Count(r => r.HasImage),
                    RemarksWithoutImage = g.Count(r => !r.HasImage)
                })
                .FirstAsync();

            var topWords = await topWordsTask;
            var remarksByDay = await remarksByDayTask;

            return new StatisticsDto(
                summary.TotalRemarks,
                summary.RemarksWithImage,
                summary.RemarksWithoutImage,
                topWords,
                remarksByDay
            );
        }
        private async Task<List<TopWordDto>> GetTopWordsAsync()
        {
            const int limit = 20;

            var result = await _dbContext.Database.SqlQuery<TopWordDto>($@"
                SELECT lexeme AS ""Word"",
                COUNT(*)::int AS ""Frequency""
                FROM (
                    SELECT unnest(tsvector_to_array(to_tsvector('russian', ""Title""))) AS lexeme
                    FROM ""Remarks""
                ) AS words
                GROUP BY lexeme
                ORDER BY COUNT(*) DESC, lexeme ASC
                LIMIT {limit};
            ").ToListAsync();

            return result;
        }
        private async Task<List<RemarksByDayDto>> GetRemarksByDayAsync()
        {
            var result = await _dbContext.Remarks
                .AsNoTracking()
                .GroupBy(r => r.CreatedAt.Date)
                .Select(g => new RemarksByDayDto(
                    DateOnly.FromDateTime(g.Key),
                    g.Count()
                ))
                .OrderBy(x => x.Day)
                .ToListAsync();

            return result;
        }
    }
}
