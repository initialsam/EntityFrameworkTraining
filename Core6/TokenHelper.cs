using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Core6;

public class TokenHelper
{
    public static TokenValidationParameters CreateTokenValidation(string issuer, string signKey)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));
        return new TokenValidationParameters
        {
            // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
            //簽發金鑰
            IssuerSigningKey = securityKey,
            //驗證 簽發金鑰
            ValidateIssuerSigningKey = true,
            //驗證 簽發者
            ValidateIssuer = true,
            ValidIssuer = issuer,
            //不驗證 接收者
            ValidateAudience = false,
            //驗證 Token 的有效期間
            ValidateLifetime = true,
            //有過期時間
            RequireExpirationTime = true,
            //時間偏移 預設五分鐘 要歸零
            ClockSkew = TimeSpan.Zero
        };
    }

    /// <summary>
    /// 驗證token是否過期
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static bool IsValidExpirationTime(string token)
    {
        JwtSecurityToken jwtSecurityToken;
        try
        {
            jwtSecurityToken = new JwtSecurityToken(token);
        }
        catch (Exception)
        {
            return false;
        }

        return jwtSecurityToken.ValidTo > DateTime.UtcNow;
    }
}