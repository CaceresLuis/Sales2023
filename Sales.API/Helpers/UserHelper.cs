using System.Text;
using Sales.API.Data;
using Sales.Shared.DTOs;
using System.Security.Claims;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Sales.API.Infrastructure.Exceptions;

namespace Sales.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly string _container;
        private readonly SalesDataContex _context;
        private readonly IFileStorage _fileStorage;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(SalesDataContex context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IConfiguration configuration, IFileStorage fileStorage)
        {
            _context = context;
            _container = "users";
            _fileStorage = fileStorage;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) => await _userManager.ConfirmEmailAsync(user, token);
        
        public async Task<string> GenerateEmailTokenConfirmAsync(User user) => await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPasswort, string newPasswort) => await _userManager.ChangePasswordAsync(user, currentPasswort, newPasswort);

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.Include(u => u.City)
                .ThenInclude(c => c.State).ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _context.Users.Include(u => u.City)
                .ThenInclude(c => c.State).ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            Task<IdentityResult> addUser = _userManager.CreateAsync(user, password);

            if (!string.IsNullOrEmpty(user.Photo))
            {
                byte[] photoUser = Convert.FromBase64String(user.Photo);
                user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
            }
            return await addUser;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(User user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                IdentityResult aa = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                Console.WriteLine(aa.Errors);
            }
        }

        public async Task<CustomResponse> UpdateUserAsync(User user)
        {
            User currentUser = await GetUserAsync(user.Email);
            if (currentUser == null)
            {
                CustomResponse result = new()
                {
                    Succeeded = false,
                    Error = "User no existe"
                };
                return result;
            }

            currentUser.CityId = user.CityId;
            currentUser.Address = user.Address ?? currentUser.Address;
            currentUser.LastName = user.LastName ?? currentUser.LastName;
            currentUser.Document = user.Document ?? currentUser.Document;
            currentUser.FirstName = user.FirstName ?? currentUser.FirstName;
            currentUser.PhoneNumber = user.PhoneNumber ?? currentUser.PhoneNumber;

            if (!string.IsNullOrEmpty(user.Photo))
            {
                byte[] photoUser = Convert.FromBase64String(user.Photo);
                if (!string.IsNullOrEmpty(currentUser.Photo))
                {
                    currentUser.Photo = await _fileStorage.EditFileAsync(photoUser, "jpg", _container, currentUser.Photo);
                }
                currentUser.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
            }            

            IdentityResult update = await _userManager.UpdateAsync(currentUser);
            if (update.Succeeded)
                return new CustomResponse() { Succeeded = true };

            return new CustomResponse() { Succeeded = false, Error = update.Errors.FirstOrDefault().ToString() };
        }

        public async Task<bool> UserExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.NormalizedEmail == email.ToLower());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginDto login)
        {
            return await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task RemoveUserAsyn(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        public TokenDto GetToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim("Address", user.Address),
                new Claim("LastName", user.LastName),
                new Claim("Document", user.Document),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("CityId", user.CityId.ToString()),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            if(user.Photo != null) claims.Add(new Claim("Photo", user.Photo));

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["JwtConfig:jwtKey"]));
            SigningCredentials credential = new(key, SecurityAlgorithms.HmacSha256);
            DateTime expiration = DateTime.UtcNow.AddDays(1);
            JwtSecurityToken token = new
                (
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: credential
                );

            return new TokenDto
            {
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
