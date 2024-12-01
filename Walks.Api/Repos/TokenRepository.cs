using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Walks.Api.Repos
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateToken(IdentityUser user, List<string> roles)
        {
            //in order to create a token.
            //configuration, claims, expires are required.


            //claims generation to be included in the payload.
            var claims = new List<Claim>(); 

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles) {
              claims.Add(new Claim(ClaimTypes.Role, role));
            
            }

            //Read and encrypt the key.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])); 

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            //genrate the token.
            var token = new JwtSecurityToken
            (
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials

            ); 


            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
