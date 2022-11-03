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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(instructorService.GetAll());
        }

        [HttpGet("courses/{InstructorId}/{SemesterId}")]
        public IActionResult GetCourses(int InstructorId, int SemesterId)
        {
            return Ok(instructorService.GetAllCourse(InstructorId, SemesterId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Instructor instructor)
        {
            await instructorService.Create(instructor);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Instructor instructor)
        {
            return Ok(instructorService.Update(instructor));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int instructorId)
        {
            try
            {
                await instructorService.Delete(instructorId);
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
