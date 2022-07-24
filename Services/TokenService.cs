using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SendMe.Models;

namespace SendMe.Services
{

    public static class TokenService
    {

        public static string GetAPI_KEY() {
           
            string secret = System.Environment.GetEnvironmentVariable("API_SENDME_SECRET");

            if (secret == null)
            {
                throw new NullReferenceException("Inválid api secret");
            }
            return secret;
        }

        public static string GenerateToken(APIConsumer consumer)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GetAPI_KEY());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, consumer.login.ToString()),
                    new Claim(ClaimTypes.Role, consumer.role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
