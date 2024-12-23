using System.Collections;
using System.Runtime.Serialization.DataContracts;
using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Application.Services;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Data;
using boilerplate_app.Infrastructure.Repositories;
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
        public UserController(IUserService userService,ApplicationDbContext dbContext, IMapper mapper) 
            {
                _userService = userService;
                _context = dbContext;
                _mapper = mapper;
            }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
           if(await UserExist(registerDto.UserName))
            {
                return BadRequest("user name exist");
            }


            var user = _mapper.Map<User>(registerDto);

            await _userService.SaveUsers(user);

            //return new UserDto 
            //{ 
            //    UserName = registerDto.UserName,
            //    Email = registerDto.Email,
            //    FullName = $"{registerDto.FirstName} {registerDto.LastName}",
            //    Password = registerDto.Password,
            //};

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        private async Task<bool> UserExist(string username)
        {
           return await _context.Users.AnyAsync(x => x.Username == username);

        }
    }
}
