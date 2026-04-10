using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
