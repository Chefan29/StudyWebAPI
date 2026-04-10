using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWebAPI.Application.DTOs
{
    public record RemarksByDayDto(
        DateOnly Day,
        int Count
        );
}
