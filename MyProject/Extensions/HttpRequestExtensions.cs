using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MyProject.Api.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Guid? GetTokenId(this HttpRequest request)
        {
            if (request.Headers.Authorization.ToString().StartsWith("Bearer "))
            {
                var token = request.Headers.Authorization.ToString().Split("Bearer ")[1];
                var handler = new JwtSecurityTokenHandler();
                var tokenObj = handler.ReadJwtToken(token);
                var jtiClaim = tokenObj.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
                if (jtiClaim != null)
                {
                    return Guid.Parse(jtiClaim.Value);
                }
            }
            return null;
        }
    }

}
