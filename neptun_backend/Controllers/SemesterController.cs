using Microsoft.AspNetCore.Mvc;
using neptun_backend.Services;

namespace neptun_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : Controller
    {
        private readonly ISemesterService semesterService;

        public SemesterController(ISemesterService _semesterService)
        {
            semesterService = _semesterService;
        }

        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(semesterService.getAll());
        }
    }
}
