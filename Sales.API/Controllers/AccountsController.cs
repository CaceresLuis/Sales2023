using AutoMapper;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Responses;
using Sales.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Sales.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly SendMailConfiguration _sendMail;

        public AccountsController(IUserHelper userHelper, IMapper mapper, IMailHelper mailHelper, IOptions<SendMailConfiguration> sendMail)
        {
            _mapper = mapper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _sendMail = sendMail.Value;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Get()
        {
            User getUser = await _userHelper.GetUserAsync(User.Identity.Name!);
            UpdateUserDto response = _mapper.Map<UpdateUserDto>(getUser);
            response.StateId = getUser.City.StateId;
            response.CountryId = getUser.City.State.CountryId;
            return Ok(response);
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

            string myToken = await _userHelper.GenerateEmailTokenConfirmAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _sendMail.UrlWEB);

            Response response = await _mailHelper.SendMail(user.FullName, user.Email,
                "Sales - Confirmacion de cuenta",
                $"<h1>Sales - Confirmacion de cuenta</h1><p>para habilitar el usuario por favor hacer<b><a href={tokenLink}> click aqui</a></b></p>");
            if (response.IsSuccess)
                return Ok(true);

            return BadRequest(response.Message);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync(LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            SignInResult result = await _userHelper.LoginAsync(login);
            if (!result.Succeeded)
                return BadRequest("usuario o contraseña invalido");

            if (result.IsLockedOut) return BadRequest("has sido bloqueado temporalmente, espera un minuto y vulve a intentar");
            if (result.IsNotAllowed) return BadRequest("El usuario ha sido inhabilitado, sigue las instrucciones enviadas a tu correo");

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

            var userUpdated = await _userHelper.GetUserAsync(user.Email);

            return Ok(_userHelper.GetToken(userUpdated));
        }

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDto changePassword)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (changePassword.NewPassword != changePassword.Confirm) return BadRequest("Las contraseñas no coinciden");

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null) return NotFound("Usuario no encontrado");

            IdentityResult change = await _userHelper.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (!change.Succeeded) return BadRequest(change.Errors.FirstOrDefault());

            return NoContent();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            token = token.Replace(" ", "+");
            User user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null) return NotFound("Usuario no encontrado");

            IdentityResult emailConfirm = await _userHelper.ConfirmEmailAsync(user, token);
            if (!emailConfirm.Succeeded) return BadRequest(emailConfirm.Errors.FirstOrDefault());

            return NoContent();
        }

        [HttpPost("ResendToken")]
        public async Task<ActionResult> ResendToken([FromBody] EmailDto email)
        {
            User user = await _userHelper.GetUserAsync(email.Email);
            if (user is null) return NotFound("Usuario no encontrado");

            string myToken = await _userHelper.GenerateEmailTokenConfirmAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userId = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _sendMail.UrlWEB);

            Response response = await _mailHelper.SendMail(user.FullName, user.Email!,
            "Saless- Confirmación de cuenta",
            $"<h1>Sales - Confirmación de cuenta</h1><p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p> <b><a href ={tokenLink}>Confirmar Email</a></b>");

            if (!response.IsSuccess) return BadRequest(response.Message);

            return NoContent();     
        }
    }
}
