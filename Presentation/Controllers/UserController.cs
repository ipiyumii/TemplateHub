using System.Collections;
using System.Runtime.Serialization.DataContracts;
using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Application.Services;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Data;
using boilerplate_app.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public UserController(IUserService userService, IJwtService jwtService, ApplicationDbContext dbContext, IMapper mapper) 
            {
                _userService = userService;
                _context = dbContext;
                _mapper = mapper;
                _jwtService = jwtService;
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
           if(await UserExist(registerDto.UserName))
            {
                return BadRequest("user name exist");
            }

            var user = await _userService.SaveUsers(registerDto);

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
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
