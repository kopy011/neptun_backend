﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(studentService.getAll());
        }
    }
}
