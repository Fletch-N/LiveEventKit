using System.Security.Claims;

namespace API.Endpoints;

public static class HttpContextExtensions
{
    public static Guid? GetCurrentUserId(this HttpContext httpContext)
    {
        string? userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out Guid parsedUserId) ? parsedUserId : null;
    }
}
