using System.Collections;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Application.Services;
using boilerplate_app.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace boilerplate_app.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService) 
            {
                _userService = userService;
            }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

    }
}
