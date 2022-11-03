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
        public IActionResult getAll()
        {
            return Ok(instructorService.getAll());
        }

        [HttpGet("courses")]
        public IActionResult getCourses([FromQuery] int InstructorId, [FromQuery] int SemesterId)
        {
            return Ok(instructorService.getAllCourse(InstructorId, SemesterId));
        }

        [HttpPost]
        public IActionResult create([FromBody] Instructor instructor)
        {
            return Ok(instructorService.Create(instructor));
        }

        [HttpPut]
        public IActionResult update([FromBody] Instructor instructor)
        {
            return Ok(instructorService.Update(instructor));
        }

        [HttpDelete]
        public IActionResult delete([FromQuery] int instructorId)
        {
            return Ok(instructorService.Delete(instructorId));
        }

        [HttpPost("take")]
        public IActionResult take([FromQuery] int InstructorId, [FromQuery] int CourseId)
        {
            try
            {
                return Ok(instructorService.takeACourse(InstructorId, CourseId));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
