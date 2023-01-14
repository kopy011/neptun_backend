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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course Course)
        {
            await courseService.Create(Course);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Course Course)
        {
            await courseService.Update(Course);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
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

        [HttpGet("{StartDate}/{EndDate}/{IgnoreFilters=false}")]
        public IActionResult getCoursesByDates(DateTime StartDate, DateTime EndDate, bool IgnoreFilters = false)
        {
            return Ok(courseService.getCoursesByDates(StartDate, EndDate, IgnoreFilters));
        }

        [Authorize(Roles = "Student, Instructor", Policy= "ActivePersonOnly")]
        [HttpGet("get/hard")]
        public IActionResult getHardCourses()
        {
            return Ok(courseService.getHardCourses());
        }
    }
}
