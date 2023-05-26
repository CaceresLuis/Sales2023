using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Sales.API.Helpers
{
    public interface IUserHelper
    {
        Task LogoutAsync();
        Task CheckRoleAsync(string roleName);
        Task<User> GetUserAsync(string email);
        Task<bool> UserExistAsync(string email);
        Task<SignInResult> LoginAsync(LoginDto login);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<IdentityResult> AddUserToRoleAsync(User user ,string roleName);
        Task RemoveUserAsyn(User user);

        TokenDto GetToken(User user);
    }
}
