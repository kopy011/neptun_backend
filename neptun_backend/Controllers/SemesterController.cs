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
    public class SemesterController : Controller
    {
        private readonly ISemesterService semesterService;

        public SemesterController(ISemesterService _semesterService)
        {
            semesterService = _semesterService;
        }

        [HttpGet("{IgnoreFilters=false}")]
        public IActionResult GetAll(bool IgnoreFilters)
        {
            return Ok(semesterService.GetAll(IgnoreFilters));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Semester Semester)
        {
            await semesterService.Create(Semester);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Semester Semester) 
        {
            await semesterService.Update(Semester);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
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
