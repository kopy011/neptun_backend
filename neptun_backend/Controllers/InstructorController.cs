﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult getCourses([FromQuery] string NeptunCode, [FromQuery] int SemesterId)
        {
            return Ok(instructorService.getAllCourse(NeptunCode, SemesterId));
        }
    }
}
