using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace boilerplate_app.Presentation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class RolesController : ControllerBase
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        public RolesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

            if (!await _roleManager.RoleExistsAsync(assignRoleDto.Role))
            {
                return BadRequest("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, assignRoleDto.Role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"Role '{assignRoleDto.Role}' assigned to user '{assignRoleDto.UserName}' successfully.");
        }


    }
}
