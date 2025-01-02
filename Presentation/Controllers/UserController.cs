
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
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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

            var user = await _userService.RegisterUser(registerDto);

            if (user == null) return BadRequest("Registration failed");
            


            return Ok(new UserDto
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var userDto = await _userService.Login(loginDto);
            if(userDto == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Fetch the User entity using the UserManager
            var user = await _userManager.FindByNameAsync(userDto.UserName);
            if(user == null)
             {
                return Unauthorized("Invalid username or password");
            }

            // Fetch user roles using UserManager
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null)
            {
                roles = new List<string>(); 
            }

            //var userDto = _mapper.Map<UserDto>(user);

           
            var jwtToken = _jwtService.GenerateJwtToken(user, roles);

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
