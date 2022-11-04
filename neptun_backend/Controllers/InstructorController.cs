using Microsoft.AspNetCore.Mvc;
using neptun_backend.Entities;
using neptun_backend.Services;

namespace neptun_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : Controller
    {
        private readonly IInstructorService instructorService;

        public InstructorController(IInstructorService _instructorService)
        {
            instructorService = _instructorService;
        }

        [HttpGet("{IgnoreFilters=false}")]
        public IActionResult GetAll(bool IgnoreFilters)
        {
            return Ok(instructorService.GetAll(IgnoreFilters));
        }

        [HttpGet("courses/{InstructorId}/{SemesterId}/{IgnoreFilters=false}")]
        public IActionResult GetCourses(int InstructorId, int SemesterId, bool IgnoreFilters)
        {
            return Ok(instructorService.GetAllCourse(InstructorId, SemesterId, IgnoreFilters));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Instructor Instructor)
        {
            await instructorService.Create(Instructor);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Instructor Instructor)
        {
            return Ok(instructorService.Update(Instructor));
        }

        [HttpDelete("{InstructorId}")]
        public async Task<IActionResult> Delete(int InstructorId)
        {
            try
            {
                await instructorService.Delete(InstructorId);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("take/{InstructorId}/{CourseId}")]
        public async Task<IActionResult> Take(int InstructorId, int CourseId)
        {
            try
            {
                await instructorService.TakeACourse(InstructorId, CourseId);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
