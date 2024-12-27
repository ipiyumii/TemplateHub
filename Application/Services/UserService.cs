using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace boilerplate_app.Application.Services
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetUsers();
        public Task<UserDto> Login(LoginDto loginDto);
        public  Task<User> SaveUsers(RegisterDto registerDto);

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

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(loginDto.UserName);
            if (user == null) return null;

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<User> SaveUsers(RegisterDto registerDto)
        {
            //var user =  PasswordHash(registerDto);
            //await _userRepository.SaveUser(user);
            //return user;

            var user = _mapper.Map<User>(registerDto);

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, registerDto.Password);
            await _userRepository.SaveUser(user);

            return user;
        }
    }
}
