
using api.Dtos;
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

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
                
            }
            return Ok();
        }
    }
}
