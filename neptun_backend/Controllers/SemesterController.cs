using Microsoft.AspNetCore.Mvc;
using neptun_backend.Entities;
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
        public IActionResult GetAll()
        {
            return Ok(semesterService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Semester Semester)
        {
            await semesterService.Create(Semester);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Semester Semester) 
        {
            await semesterService.Update(Semester);
            return Ok();
        }

        [HttpDelete("{SemesterId}")]
        public async Task<IActionResult> Delete(int SemesterId)
        {
            try
            {
                await semesterService.Delete(SemesterId);
                return Ok();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
