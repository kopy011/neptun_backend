using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neptun_backend.Entities;
using neptun_backend.Services;
using neptun_backend.Utils;

namespace neptun_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Instructor, Student")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Instructor Instructor)
        {
            await instructorService.Create(Instructor);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult Update([FromBody] Instructor Instructor)
        {
            return Ok(instructorService.Update(Instructor));
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [HttpGet("students/{InstructorId}/{SemesterId}")]
        public IActionResult GetAllStudents(int InstructorId, int SemesterId)
        {
            return Ok(instructorService.GetAllStudents(InstructorId, SemesterId));
        }

        [HttpGet("semester-statistics/{InstructorId}")]
        public IActionResult GetSemesterStatistics(int InstructorId)
        {
            return Ok(instructorService.GetSemesterStatistics(InstructorId));
        }
    }
}
