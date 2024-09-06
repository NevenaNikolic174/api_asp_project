using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public class AuthorizeRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly int _roleId;

    public AuthorizeRoleAttribute(int roleId)
    {
        _roleId = roleId;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var roleIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId");

        if (roleIdClaim == null || !int.TryParse(roleIdClaim.Value, out var roleId) || roleId != _roleId)
        {
            context.Result = new ForbidResult();
        }
    }
}
