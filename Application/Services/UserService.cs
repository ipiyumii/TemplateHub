using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace boilerplate_app.Application.Services
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetUsers();
        public Task<UserDto> Login(LoginDto loginDto);
        public UserDto GetUser(string username);
        public  Task<User> SaveUsers(RegisterDto registerDto);
        public User PasswordHash(RegisterDto registerDto);
    
    }

    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
           
        }

        public UserDto GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(loginDto.UserName);
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public User PasswordHash(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            return user;

        }

        public async Task<User> SaveUsers(RegisterDto registerDto)
        {
            var user =  PasswordHash(registerDto);
            await _userRepository.SaveUser(user);
            return user;
        }
    }
}
