using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neptun_backend.DTOS.ApplicationUserDTOS;
using neptun_backend.Services;

namespace neptun_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("init")]
        public async Task<IActionResult> Init()
        {
            try
            {
                await _userService.InitRoles();
                await _userService.InitUsers();
                return Ok();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                await _userService.Register(registerRequest);
                return Ok();
            }   catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
