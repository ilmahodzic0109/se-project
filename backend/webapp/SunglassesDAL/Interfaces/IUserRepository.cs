using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunglassesDAL.Model;

namespace SunglassesDAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
        void UpdateUser(User user);
        Task SaveAsync();
    }
}