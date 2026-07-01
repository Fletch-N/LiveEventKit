using Domain;
using Microsoft.AspNetCore.Identity;

namespace API.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.MapIdentityApi<KitUser>();

        group.MapPost("/logout", Logout)
            .WithName("Logout")
            .RequireAuthorization();

        group.MapGet("/me", GetCurrentUser)
            .WithName("GetCurrentUser")
            .RequireAuthorization();

        return group;
    }

    private static async Task<IResult> Logout(
        SignInManager<KitUser> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }

    private static async Task<IResult> GetCurrentUser(
        UserManager<KitUser> userManager,
        HttpContext httpContext)
    {
        KitUser? user = await userManager.GetUserAsync(httpContext.User);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        IList<string> roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            Roles = roles
        });
    }
}
