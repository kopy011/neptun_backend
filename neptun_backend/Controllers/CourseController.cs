using Microsoft.AspNetCore.Mvc;
using neptun_backend.Entities;
using neptun_backend.Services;

namespace neptun_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService _courseService)
        {
            courseService = _courseService;
        }

        [HttpGet("{IgnoreFilters=false}")]
        public IActionResult getAll(bool IgnoreFilters)
        {
            return Ok(courseService.GetAll(IgnoreFilters));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course Course)
        {
            await courseService.Create(Course);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Course Course)
        {
            await courseService.Update(Course);
            return Ok();
        }

        [HttpDelete("{CourseId}")]
        public async Task<IActionResult> Delete(int CourseId)
        {
            try
            {
                await courseService.Delete(CourseId);
                return Ok();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
