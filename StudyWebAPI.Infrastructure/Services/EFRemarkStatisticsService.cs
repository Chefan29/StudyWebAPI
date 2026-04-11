using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StudyWebAPI.Application.DTOs;
using StudyWebAPI.Application.Interfaces;
using StudyWebAPI.Infrastructure.Data;
using System.Text.Json;

namespace StudyWebAPI.Infrastructure.Services
{
    public class EFRemarkStatisticsService : IRemarkStatisticsService
    {
        private readonly RemarkDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public EFRemarkStatisticsService(RemarkDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<StatisticsDto> GetStatisticsAsync()
        {
            const string cacheKey = "remarks:statistics";
            var cachedJson = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(cachedJson))
            {
                var cachedStatistics = JsonSerializer.Deserialize<StatisticsDto>(cachedJson);

                if (cachedStatistics is not null)
                {
                    return cachedStatistics;
                }
            }

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

            var topWords = await GetTopWordsAsync();
            var remarksByDay = await GetRemarksByDayAsync();

            var statistics = new StatisticsDto(
                summary.TotalRemarks,
                summary.RemarksWithImage,
                summary.RemarksWithoutImage,
                topWords,
                remarksByDay
            );
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            var json = JsonSerializer.Serialize(statistics);

            await _cache.SetStringAsync(cacheKey, json, cacheOptions);

            return statistics;
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
            var rawResult = await _dbContext.Remarks
                .AsNoTracking()
                .GroupBy(r => r.CreatedAt.Date)
                .Select(g => new
                {
                    Day = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Day)
                .ToListAsync();

            var result = rawResult
                .Select(x => new RemarksByDayDto(
                    DateOnly.FromDateTime(x.Day),
                    x.Count
                ))
                .ToList();

            return result;
        }
    }
}
