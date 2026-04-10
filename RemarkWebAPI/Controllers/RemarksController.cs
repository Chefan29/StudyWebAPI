using StudyWebAPI.Application.Interfaces;
using StudyWebAPI.Application.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace RemarkWebAPI.Controllers
{
    [ApiController]
    [Route("api/remarks")]
    public class RemarksController : ControllerBase
    {
        private readonly IRemarkQueryService _remarkQueryService;
        private readonly IRemarkComandService _remarkComandService;
        public RemarksController(IRemarkQueryService remarkQueryService, IRemarkComandService remarkComandService)
        {
            _remarkQueryService = remarkQueryService;
            _remarkComandService = remarkComandService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _remarkQueryService.GetAllAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _remarkQueryService.GetByIdAsync(id);
            if (!result.ok)
            {
                return NotFound(result.error);
            }
            return Ok(result.remarkDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateDto createUpdateDto)
        {
            var result = await _remarkComandService.CreateAsync(createUpdateDto);
            if (!result.ok)
            {
                return BadRequest(result.error);
            }
            return CreatedAtAction(nameof(GetById), new { id = result.remarkDto!.Id }, result.remarkDto);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update (int id, CreateUpdateDto updateDto)
        {
            var result = await _remarkComandService.UpdateAsync(id, updateDto);
            if (!result.ok)
            {
                return BadRequest(result.error);
            }
            return Ok(result.remarkDto);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete (int id)
        {
            if (!await _remarkComandService.DeleteAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            var result = await _remarkQueryService.SearchAsync(query);
            if (!result.ok)
            {
                return BadRequest(result.error);
            }
            return Ok(result.remarkDtos);
        }
    }
}
