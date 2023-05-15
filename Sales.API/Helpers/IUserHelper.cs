using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Sales.API.Helpers
{
    public interface IUserHelper
    {
        Task CheckRoleAsync(string roleName);
        Task<User> GetUserAsync(string email);
        Task AddUserToRoleAsync(User user ,string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<IdentityResult> AddUserAsync(User user, string password);
        //Task<IdentityUser> GetCurrentUserAsync();
    }
}
