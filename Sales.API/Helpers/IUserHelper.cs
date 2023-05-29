using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Sales.API.Infrastructure.Exceptions;

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
        Task<User> GetUserAsync(Guid id);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPasswort, string newPasswort);
        Task<CustomResponse> UpdateUserAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GenerateEmailTokenConfirmAsync(User user);
    }
}
