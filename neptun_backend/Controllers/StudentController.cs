using Microsoft.AspNetCore.Mvc;
using neptun_backend.Entities;
using neptun_backend.Services;

namespace neptun_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService _studentService)
        {
            studentService = _studentService;
        }

        [HttpGet("{IgnoreFilters=false}")]
        public IActionResult GetAll(bool IgnoreFilters)
        {
            return Ok(studentService.GetAll(IgnoreFilters));
        }

        [HttpGet("courses/{StudentId}/{SemesterId}/{IgnoreFilters=false}")]
        public IActionResult GetAllCourse(int StudentId, int SemesterId, bool IgnoreFilters)
        {
            return Ok(studentService.GetAllCourse(StudentId, SemesterId, IgnoreFilters));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Student Student)
        {
            await studentService.Create(Student);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Student Student)
        {
            await studentService.Update(Student);
            return Ok();
        }

        [HttpDelete("{StudentId}")]
        public async Task<IActionResult> Delete(int StudentId)
        {
            try
            {
                await studentService.Delete(StudentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("take/{StudentId}/{CourseId}")]
        public async Task<IActionResult> Take(int StudentId, int CourseId)
        {
            try
            {
                await studentService.TakeACourse(StudentId, CourseId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
