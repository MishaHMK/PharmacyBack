using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Pharmacy.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pharmacy.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _iconfiguration;
        public JWTService(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public Token Authenticate(string id, string name, IEnumerable<string> roles)
        {
            var role = roles.FirstOrDefault();

            var claims = new Claim[]
             {
                 new Claim("NameIdentifier", id),
                 new Claim("UserName", name),
                 new Claim("Role", role)
             };

            // Generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(180),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { TokenString = tokenHandler.WriteToken(token) };

        }
    }
}
