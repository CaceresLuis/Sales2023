using System.Text;
using Sales.API.Data;
using Sales.Shared.DTOs;
using System.Security.Claims;
using Sales.API.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Sales.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly JwtKey _options;
        private readonly SalesDataContex _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(SalesDataContex context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IOptions<JwtKey> options, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _options = options.Value;
            _configuration = configuration;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
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

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.Include(u => u.City)
                .ThenInclude(c => c.State).ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(u => u.Email == email);
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
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim("Document", user.Document),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Address", user.Address),
                new Claim("Photo", user.Photo?? string.Empty),
                new Claim("CityId", user.CityId.ToString()),
            };

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
