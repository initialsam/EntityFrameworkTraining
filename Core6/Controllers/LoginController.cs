using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

namespace Core6.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("Create")]
        public IActionResult Create(string memberId)
        {
            var jwt = CreateToken(memberId, 7);
            return Ok(jwt);
        }
        [Authorize]
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok(new
            {
                Claims = User.Claims.Select(p => new { p.Type, p.Value })
            });
        }
        private string CreateToken(string memberId, int expireDays)
        {
            var claims = new List<Claim>();

            var jwtId = Guid.NewGuid().ToString();
            // User.Identity.Name
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, memberId));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwtId));
            // 你可以自行擴充 "roles" 加入登入者該有的角色
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            //claims.Add(new Claim("roles", "Users"));

            // 建立 Jwt 參數
            var userClaimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is key for your setting"));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "TestApi",
                //Expires = DateTime.Now.AddDays(expireDays),
                Subject = userClaimsIdentity,
                SigningCredentials = signingCredentials
            };
            // 建立 Jwt Token
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }
    }

}
