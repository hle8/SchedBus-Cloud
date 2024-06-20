using System.Security.Claims;

namespace SchedBus.Extensions;

public static class UserExtension
{
    public static string GetUserEmail(this ClaimsPrincipal user) => user.FindFirst(c => c.Type == "email").Value;
}
