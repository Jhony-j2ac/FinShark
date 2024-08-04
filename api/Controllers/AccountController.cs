using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace api.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<AppUser> _userManager { get; set; }
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) {
                return Unauthorized("Invalid username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) {  return Unauthorized("Username not found and/or password incorrect"); }

            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        } 

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                string errorMsg = "";
                if (!ModelState.IsValid) { 
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,

                };

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password );

                if (createUser.Succeeded) {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (!roleResult.Succeeded) {
                        errorMsg = string.Join(",", roleResult.Errors.Select(x => x.Description));
                    }

                    return Ok(new NewUserDto { 
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    });

                }
                else
                {
                    errorMsg = string.Join(",", createUser.Errors.Select(x => x.Description));
                }

                if (!string.IsNullOrWhiteSpace(errorMsg))
                {
                    return StatusCode(500, errorMsg);
                }
            }
            catch (Exception ex) { 
                    return StatusCode(500, ex.ToString());
            }
            return Ok();
        }
    }
}
