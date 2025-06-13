using SunglassesDAL.Model;

namespace Sunglasses.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> LoginUserAsync(string email, string password);
        Task UpdateUserAsync(User user);
        Task LogoutUserAsync(Guid userId);
        Task<User?> GetUserByIdAsync(Guid userId);
    }
}