using StudyWebAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyWebAPI.Application.Interfaces
{
    public interface IRemarkComandService
    {
        Task<(bool ok, string? error, RemarkDto? remarkDto)> CreateAsync(CreateUpdateDto createUpdateDto);
        Task<(bool ok, string? error, RemarkDto? remarkDto)> UpdateAsync(int id, CreateUpdateDto createUpdateDto);
        Task<bool> DeleteAsync(int id);
    }
}
