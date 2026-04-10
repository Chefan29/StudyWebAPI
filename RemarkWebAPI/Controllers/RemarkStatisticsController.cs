using Microsoft.AspNetCore.Mvc;
using StudyWebAPI.Application.Interfaces;

namespace RemarkWebAPI.Controllers
{
    [ApiController]
    [Route("api/remarkStatistics")]
    public class RemarkStatisticsController : ControllerBase
    {
        private readonly IRemarkStatisticsService _remarkStatisticsService;
        public RemarkStatisticsController(IRemarkStatisticsService remarkStatisticsService)
        {
            _remarkStatisticsService = remarkStatisticsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetStatisticsAsync()
        {
            return Ok( await _remarkStatisticsService.GetStatisticsAsync());
        }
    }
}
