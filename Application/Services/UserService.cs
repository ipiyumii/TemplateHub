using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Repositories;

namespace boilerplate_app.Application.Services
{
    public interface IUserService
    {
        public List<UserDto> GetUsers();
        public UserDto GetUser(string username);
        public  Task<bool> SaveUsers(User user);

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

        public List<UserDto> GetUsers()
        {
            var users = _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<bool> SaveUsers(User user)
        {
            return await _userRepository.SaveUser(user);

        }
    }
}
