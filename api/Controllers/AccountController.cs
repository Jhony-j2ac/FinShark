using api.Dtos.Account;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        private static Dictionary<string, RefreshToken> _refreshTokens = new();

        public static Dictionary<string, RefreshToken> RefreshTokens
        {
            get => _refreshTokens;
            set => _refreshTokens = value;
        }

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

            var token = _tokenService.CreateToken(user);
            var refreshToken = ITokenService.CreateRefreshToken();

            ///  Para permitir  que la coockie la reciba el navegador, se debe establecer que samesite sea en none, esto para pruebas donde el puerto es diferente.
            ///  Adicional para que esto funcione debe esta configurado secure como true, lo que requiere que front y back tengan https 
            ///  Nota: se hizo prueba haciendo que el front no sea https y lo permite igual, pero podria fallar en algun momento
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // ⚠️ Obligatorio para SameSite=None
                SameSite = SameSiteMode.None, // ⚠️ Permite envío cross-site
                //Expires = DateTimeOffset.UtcNow.AddMinutes(2)
                Expires = DateTime.Now.AddHours(1)
            });

            /// cuando esten en el mismo sitio se debe usar strict, lax depende del navegador, pueden cambiar las limitaciones
            //Response.Cookies.Append("token", token, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true, // ⚠️ Obligatorio para SameSite=None
            //    SameSite = SameSiteMode.Lax, // ⚠️ Permite envío cross-site
            //    Expires = DateTimeOffset.UtcNow.AddDays(7)
            //});


            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true en producción
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                Path = "/api/Account",
            });

            RefreshTokens.Add(refreshToken, new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                User = user,
            });

            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Append("token", "", new CookieOptions  
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1), // Borra la cookie
                Path = "/" // Asegúrate de usar el mismo path que usaste al crearla
            });

            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true en producción
                SameSite = SameSiteMode.None,
                Path = "/api/Account",
                Expires = DateTimeOffset.UtcNow.AddDays(-1), // Borra la cookie
            });

            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {

            var requestToken = Request.Cookies["refreshToken"];
            if (requestToken == null)
                return Unauthorized();


            RefreshToken refreshToken;
            if (!RefreshTokens.TryGetValue(requestToken, out refreshToken))
            {
                return Unauthorized();
            }

            var appUser = refreshToken.User;

            var newAccessToken = _tokenService.CreateToken(appUser);
            var newRefreshToken = ITokenService.CreateRefreshToken();

            refreshToken.ReplacedByToken = refreshToken.Token;
            refreshToken.Token = newRefreshToken;

            RefreshTokens.Remove(requestToken);
            RefreshTokens.Add(newRefreshToken, refreshToken);

            ///  Se envia el nuevo token de acceso, y luego el nuevo refresh token
            Response.Cookies.Append("token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // ⚠️ Obligatorio para SameSite=None
                SameSite = SameSiteMode.None, // ⚠️ Permite envío cross-site
                //Expires = DateTimeOffset.UtcNow.AddDays(1)
                Expires = DateTime.Now.AddHours(1)
            });

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true en producción
                SameSite = SameSiteMode.None,
                Path = "/api/Account",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new NewUserDto
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                Token = newAccessToken
            });
        }

    }
}
