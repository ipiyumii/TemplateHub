using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Infrastructure.Repositories;

namespace boilerplate_app.Application.Services
{
    public interface IUserService
    {
        public List<UserDto> GetUsers();
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

        public List<UserDto> GetUsers()
        {
            var users = _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
