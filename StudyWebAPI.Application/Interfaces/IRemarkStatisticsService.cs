using StudyWebAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWebAPI.Application.Interfaces
{
    public interface IRemarkStatisticsService
    {
        Task<StatisticsDto> GetStatisticsAsync();
    }
}
