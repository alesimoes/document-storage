using DocStorage.Application.Adapters;
using DocStorage.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocStorage.Service.Authorization
{
    public class JwtUtils : IJwtUtils
    {
        private string secretKey = "BE2366F8-9E9A-4A3A-BB33-CB98D5429D64";
        public string GenerateJwtToken(User user)
        {
            //10 Days validation token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var claims = new[]
                                {
                                    new Claim(ClaimTypes.Name, user.Username),
                                    new Claim("id", user.Id.ToString()),
                                    new Claim(ClaimTypes.Role, user.Role.ToString())
                                };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
