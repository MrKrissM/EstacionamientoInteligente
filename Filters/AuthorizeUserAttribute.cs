// Filters/AuthorizeUserAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EstacionamientoInteligente.Filters;

public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }
    }
}