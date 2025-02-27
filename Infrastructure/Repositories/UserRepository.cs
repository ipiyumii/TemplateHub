﻿using System;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace boilerplate_app.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        User GetUserbyId(int id);
       
        void DeleteUser(int id);
        void UpdateUser(User user);
        Task<User> GetUserByUserNameAsync(string username);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users.ToListAsync(); 
            return users;
        }

        public User GetUserbyId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

    

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
