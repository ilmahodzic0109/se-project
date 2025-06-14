using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunglasses.Services.Interfaces;
using SunglassesDAL.Implementations;
using SunglassesDAL.Interfaces;
using SunglassesDAL.Model;

namespace Sunglasses.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        public UserService(IUserRepository userRepository, ICartRepository cartRepository)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
        }
        public async Task<User> RegisterUserAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("Email already in use");
            }
            var hashedPassword = HashPassword(password);
            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                IsLogged = false,
                IsAdmin = false
            };

            var savedUser = await _userRepository.AddUserAsync(user);
            return savedUser;
        }
        public async Task<User> LoginUserAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);

            if (existingUser == null)
            {
                throw new Exception("Invalid credentials");
            }

            if (!VerifyPassword(password, existingUser.Password))
            {
                throw new Exception("Invalid credentials");
            }
            existingUser.IsLogged = true;
            return existingUser;
        }

        public async Task LogoutUserAsync(Guid userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }
            existingUser.IsLogged = false;
            _userRepository.UpdateUser(existingUser);
            await _userRepository.SaveAsync();
            await _cartRepository.ClearCartForUserAsync(userId);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        private bool VerifyPassword(string providedPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedHashedPassword);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.UpdateUser(user);
            await _userRepository.SaveAsync();
        }
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
    }
}