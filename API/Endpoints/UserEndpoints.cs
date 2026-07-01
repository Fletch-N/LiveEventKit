using Application.Attendance;
using Application.Following;
using Application.Users;

namespace API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", ListUsers)
            .WithName("ListUsers")
            .RequireAuthorization();

        group.MapGet("/me", GetCurrentUserProfile)
            .WithName("GetCurrentUserProfile")
            .RequireAuthorization();

        group.MapPut("/me", UpdateCurrentUserProfile)
            .WithName("UpdateCurrentUserProfile")
            .RequireAuthorization();

        group.MapGet("/me/attendances", GetCurrentUserAttendances)
            .WithName("GetCurrentUserAttendances")
            .RequireAuthorization();

        group.MapGet("/{id:guid}", GetUserProfileById)
            .WithName("GetUserProfileById")
            .RequireAuthorization();

        group.MapGet("/{id:guid}/followers", GetFollowers)
            .WithName("GetFollowers")
            .RequireAuthorization();

        group.MapGet("/{id:guid}/following", GetFollowing)
            .WithName("GetFollowing")
            .RequireAuthorization();

        group.MapPost("/{id:guid}/follow", FollowUser)
            .WithName("FollowUser")
            .RequireAuthorization();

        group.MapDelete("/{id:guid}/follow", UnfollowUser)
            .WithName("UnfollowUser")
            .RequireAuthorization();

        return group;
    }

    private static async Task<IResult> ListUsers(
        string? search,
        int? page,
        int? limit,
        ListUsers.Handler handler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ListUsers.Response> result = await handler.Handle(
            new ListUsers.Request(search, page ?? 0, limit ?? 25),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetCurrentUserProfile(
        HttpContext httpContext,
        GetUserProfile.Handler handler,
        CancellationToken cancellationToken)
    {
        Guid? userId = httpContext.GetCurrentUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        GetUserProfile.Response? result = await handler.Handle(
            new GetUserProfile.Request(userId.Value),
            cancellationToken);

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> UpdateCurrentUserProfile(
        HttpContext httpContext,
        UpdateUserProfile.Request request,
        UpdateUserProfile.Handler handler,
        CancellationToken cancellationToken)
    {
        Guid? userId = httpContext.GetCurrentUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        UpdateUserProfile.Response? result = await handler.Handle(
            request with { Id = userId.Value },
            cancellationToken);

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> GetCurrentUserAttendances(
        HttpContext httpContext,
        GetUserAttendances.Handler handler,
        CancellationToken cancellationToken)
    {
        Guid? userId = httpContext.GetCurrentUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        IReadOnlyList<GetUserAttendances.Response> result = await handler.Handle(
            new GetUserAttendances.Request(userId.Value),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetUserProfileById(
        Guid id,
        GetUserProfile.Handler handler,
        CancellationToken cancellationToken)
    {
        GetUserProfile.Response? result = await handler.Handle(new GetUserProfile.Request(id), cancellationToken);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> GetFollowers(
        Guid id,
        ListFollowers.Handler handler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ListFollowers.Response> result = await handler.Handle(
            new ListFollowers.Request(id),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetFollowing(
        Guid id,
        ListFollowing.Handler handler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ListFollowing.Response> result = await handler.Handle(
            new ListFollowing.Request(id),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> FollowUser(
        Guid id,
        HttpContext httpContext,
        FollowUser.Handler handler,
        CancellationToken cancellationToken)
    {
        Guid? userId = httpContext.GetCurrentUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        try
        {
            bool created = await handler.Handle(new FollowUser.Request(userId.Value, id), cancellationToken);
            return created ? Results.Ok(new { targetUserId = id, following = true }) : Results.Ok(new { targetUserId = id, following = true, alreadyExisted = true });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> UnfollowUser(
        Guid id,
        HttpContext httpContext,
        UnfollowUser.Handler handler,
        CancellationToken cancellationToken)
    {
        Guid? userId = httpContext.GetCurrentUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        bool removed = await handler.Handle(new UnfollowUser.Request(userId.Value, id), cancellationToken);
        return removed ? Results.NoContent() : Results.NotFound();
    }
}
