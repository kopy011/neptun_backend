using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(courseService.getAll());
        }
    }
}
