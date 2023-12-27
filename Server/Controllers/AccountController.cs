using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase

    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        public AccountController(IAccountRepository accountRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (await _accountRepository.UserExists(registerDto.Username))
            {
                return BadRequest("Username is taken");
            }

            _accountRepository.AddUser(registerDto);
            await _accountRepository.SaveAllAsync();
            return Ok(new { message = "Registration successful" });
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _accountRepository.GetUserByUsernameAsync(loginDto.Username);
            if (user == null)
            {
                return Unauthorized("invalid username");
            }

            if (!_accountRepository.ValidatePassword(loginDto.Password, user.PasswordSalt, user.PasswordHash))
            {
                return Unauthorized("invalid password");
            }

            return new UserDto(user.Id,user.Username,_tokenService.CreateToken(user));
        }
    }
}