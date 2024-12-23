using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Data;

namespace boilerplate_app.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetUserbyId(int id);
        Task<bool> SaveUser(User user);
        void DeleteUser(int id);
        void UpdateUser(User user);
    }

    public class UserRepository : IUserRepository
    {
        ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>
            {
                //new User
                //{
                //    Id = 1,
                //    Username = "johndoe",
                //    FirstName = "John",
                //    LastName = "Doe",
                //    Email = "johndoe@example.com",
                //    Password = "password123"
                //},
                //new User
                //{
                //    Id = 2,
                //    Username = "janedoe",
                //    FirstName = "Jane",
                //    LastName = "Doe",
                //    Email = "janedoe@example.com",
                //    Password = "password456"
                //}
            };
            return users;
        }

        public User GetUserbyId(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

       

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
