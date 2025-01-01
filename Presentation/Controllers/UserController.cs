
using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Application.Services;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace boilerplate_app.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        IUserService _userService;
        ApplicationDbContext _context;
        IMapper _mapper;
        IJwtService _jwtService;
        UserManager<User> _userManager;
        public UserController(IUserService userService, IJwtService jwtService, ApplicationDbContext dbContext, IMapper mapper, UserManager<User> userManager)
            {
                _userService = userService;
                _context = dbContext;
                _mapper = mapper;
                _jwtService = jwtService;
                _userManager = userManager;
            }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //if (await UserExist(registerDto.UserName))
            //{
            //    return BadRequest("user name exist");
            //}

            //var user = await _userService.SaveUsers(registerDto);

            //var userDto = _mapper.Map<UserDto>(user);
            //return Ok(userDto);

            if (await _userManager.FindByNameAsync(registerDto.UserName) != null)
            {
                return BadRequest("User name already exists");
            }

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign default role (User)
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new
            {
                Message = "User registered successfully!",
                UserId = user.Id,
                AssignedRole = "User"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.Login(loginDto);
            if(user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var userDto = _mapper.Map<UserDto>(user);

            var jwtToken = _jwtService.GenerateJwtToken(user);

            // Return UserDto with token
            return Ok(new
            {
                Token = jwtToken,
                User = user
            });
        }

        private async Task<bool> UserExist(string username)
        {
           return await _context.Users.AnyAsync(x => x.UserName == username);

        }
    }
}
