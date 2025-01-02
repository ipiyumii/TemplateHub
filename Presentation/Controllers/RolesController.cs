using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;

namespace boilerplate_app.Presentation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class RolesController : ControllerBase
    {
        UserManager<User> _userManager;
        public RolesController(UserManager<User> userManager) 
        {
            _userManager = userManager;
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AssignRole(AssignRoleDto assignRoleDto)
        {
            var user = await _userManager.FindByNameAsync(assignRoleDto.UserName);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if ()
        }


    }
}
