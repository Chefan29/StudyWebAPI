using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWebAPI.Application.DTOs
{
    public record CreateUpdateDto (string Title, string RemarkContent, bool HasImage);
}
