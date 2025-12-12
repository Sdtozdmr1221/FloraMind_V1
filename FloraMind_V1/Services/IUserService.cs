using FloraMind_V1.Services;
using FloraMind_V1.Models;
using Microsoft.EntityFrameworkCore;

public interface IUserService
    {

    Task<List<User>> GetAllUsersAsync();

    Task UpdateUserRole(int userId, string NewRole);

    Task UpdateLastLoginDateAsync(int userId);

    Task<IEnumerable<User>> GetUsersAsync(string searchString = null);

    Task ToggleBanStatusAsync(int userId, bool isBanned);

    Task<User> GetUserByIdAsync(int userId);

}

