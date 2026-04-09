using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWebAPI.Application.DTOs
{
    public record RemarkDto(
        int Id,
        string Title,
        string RemarkContent,
        DateTime CreatedAt,
        bool HasImage
    );
}
