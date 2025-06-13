using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SunglassesDAL.Interfaces;
using SunglassesDAL.Model;

namespace SunglassesDAL.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly WebshopContext _context;

        public UserRepository(WebshopContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }


        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}