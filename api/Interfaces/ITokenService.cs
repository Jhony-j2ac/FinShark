using api.Models;
using System.Security.Cryptography;

namespace api.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser appUser);
        public static string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
