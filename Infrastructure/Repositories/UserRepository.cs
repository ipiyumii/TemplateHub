using boilerplate_app.Core.Entities;

namespace boilerplate_app.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetUserbyId(int id);
        void InsertUser(User user);
        void DeleteUser(int id);
        void UpdateUser(User user);
    }

    public class UserRepository : IUserRepository
    {
        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "johndoe",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com",
                    Password = "password123"
                },
                new User
                {
                    Id = 2,
                    Username = "janedoe",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "janedoe@example.com",
                    Password = "password456"
                }
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

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
