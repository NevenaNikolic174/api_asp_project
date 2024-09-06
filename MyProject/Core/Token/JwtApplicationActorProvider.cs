using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using MyProject.Application.Actor;
using MyProject.Implementation;
using Newtonsoft.Json;

namespace MyProject.Api.Core.Token
{
    public class JwtApplicationActorProvider : IApplicationActorProvider
    {
        private readonly string _authorizationHeader;

        public JwtApplicationActorProvider(string authorizationHeader)
        {
            _authorizationHeader = authorizationHeader;
        }

        public IApplicationActor GetActor()
        {
            if (string.IsNullOrEmpty(_authorizationHeader) || !_authorizationHeader.StartsWith("Bearer "))
            {
                return new UnauthorizedActor();
            }

            try
            {
                // Ekstrahujte token iz authorizationHeader
                string token = _authorizationHeader.Substring("Bearer ".Length).Trim();

                var handler = new JwtSecurityTokenHandler();
                var tokenObj = handler.ReadJwtToken(token);

                var claims = tokenObj.Claims.ToList();

                // Ekstrahujte sve potrebne claim-ove
                var actor = new Actor
                {
                    Username = claims.FirstOrDefault(x => x.Type == "Username")?.Value,
                    Email = claims.FirstOrDefault(x => x.Type == "Email")?.Value,
                    FirstName = claims.FirstOrDefault(x => x.Type == "FirstName")?.Value,
                    LastName = claims.FirstOrDefault(x => x.Type == "LastName")?.Value,
                    Id = int.Parse(claims.FirstOrDefault(x => x.Type == "Id")?.Value ?? "0"),
                    RoleId = int.Parse(claims.FirstOrDefault(x => x.Type == "RoleId")?.Value ?? "0"),   
                    AllowedUseCases = JsonConvert.DeserializeObject<List<int>>(claims.FirstOrDefault(x => x.Type == "UseCaseIds")?.Value ?? "[]")
                };

                return actor;
            }
            catch (Exception)
            {
                // U slučaju greške, vratite UnauthorizedActor
                return new UnauthorizedActor();
            }
        }
    }
}
