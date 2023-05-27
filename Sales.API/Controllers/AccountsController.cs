using AutoMapper;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sales.API.Infrastructure.Exceptions;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserHelper _userHelper;

        public AccountsController(IUserHelper userHelper, IMapper mapper)
        {
            _mapper = mapper;
            _userHelper = userHelper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Get()
        {
            return Ok(await _userHelper.GetUserAsync(User.Identity.Name!));
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid || userDto.Password != userDto.PasswordConfirm)
                return BadRequest();

            if (await _userHelper.UserExistAsync(userDto.Email))
                return BadRequest("The user mail aready exist");

            User user = _mapper.Map<User>(userDto);
            user.UserName = userDto.Email;
            IdentityResult create = await _userHelper.AddUserAsync(user, userDto.Password);
            if (!create.Succeeded)
                return BadRequest(create.Errors.FirstOrDefault());

            IdentityResult addUserToRol = await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
            if (!addUserToRol.Succeeded)
            {
                await _userHelper.RemoveUserAsyn(user);
                return BadRequest(addUserToRol.Errors.FirstOrDefault());
            }

            return Ok(create.Succeeded);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync(LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            SignInResult result = await _userHelper.LoginAsync(login);
            if (!result.Succeeded)
                return BadRequest();

            User user = await _userHelper.GetUserAsync(login.Email);
            if (user == null)
                return BadRequest("Contraseña o correo erroneos");

            return Ok(_userHelper.GetToken(user));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Update(UpdateUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            CustomResponse update = await _userHelper.UpdateUserAsync(user);
            if (!update.Succeeded)
                return BadRequest(update.Error);

            return NoContent();
        }
    }
}
