using Application.Attendance;
using Application.Sessions;

namespace API.Endpoints;

public static class SessionEndpoints
{
    public static RouteGroupBuilder MapSessionEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", ListSessions)
            .WithName("ListSessions");

        group.MapGet("/{id:guid}", GetSessionById)
            .WithName("GetSessionById");

        group.MapPost("/", CreateSession)
            .WithName("CreateSession")
            .RequireAuthorization("EventWriteAccess");

        group.MapPut("/{id:guid}", UpdateSession)
            .WithName("UpdateSession")
            .RequireAuthorization("EventWriteAccess");

        group.MapDelete("/{id:guid}", DeleteSession)
            .WithName("DeleteSession")
            .RequireAuthorization("EventWriteAccess");

        group.MapGet("/{id:guid}/attendees", GetSessionAttendees)
            .WithName("GetSessionAttendees")
            .RequireAuthorization("EventWriteAccess");

        group.MapPost("/{id:guid}/register", RegisterAttendance)
            .WithName("RegisterAttendance")
            .RequireAuthorization();

        group.MapDelete("/{id:guid}/register", CancelAttendance)
            .WithName("CancelAttendance")
            .RequireAuthorization();

        return group;
    }

    private static async Task<IResult> ListSessions(
        Guid? eventId,
        ListSessions.Handler handler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ListSessions.Response> result = await handler.Handle(
            new ListSessions.Request(eventId),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetSessionById(
        Guid id,
        GetSession.Handler handler,
        CancellationToken cancellationToken)
    {
        GetSession.Response? result = await handler.Handle(
            new GetSession.Request(id),
            cancellationToken);

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> CreateSession(
        CreateSession.Request request,
        CreateSession.Handler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            CreateSession.Response result = await handler.Handle(request, cancellationToken);

            return Results.Created($"/api/sessions/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> UpdateSession(
        Guid id,
        UpdateSession.Request request,
        UpdateSession.Handler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            UpdateSession.Response? result = await handler.Handle(
                request with { Id = id },
                cancellationToken);

            return result is null ? Results.NotFound() : Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> DeleteSession(
        Guid id,
        DeleteSession.Handler handler,
        CancellationToken cancellationToken)
    {
        bool deleted = await handler.Handle(new DeleteSession.Request(id), cancellationToken);
        return deleted ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> GetSessionAttendees(
        Guid id,
        GetSessionAttendees.Handler handler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<GetSessionAttendees.Response> result = await handler.Handle(
            new GetSessionAttendees.Request(id),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> RegisterAttendance(
        Guid id,
        HttpContext httpContext,
        RegisterAttendance.Handler handler,
        CancellationToken cancellationToken)
    {
        Guid? userId = httpContext.GetCurrentUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        bool created = await handler.Handle(new RegisterAttendance.Request(userId.Value, id), cancellationToken);
        return created ? Results.Ok(new { sessionId = id, registered = true }) : Results.Ok(new { sessionId = id, registered = true, updated = true });
    }

    private static async Task<IResult> CancelAttendance(
        Guid id,
        HttpContext httpContext,
        CancelAttendance.Handler handler,
        CancellationToken cancellationToken)
    {
        Guid? userId = httpContext.GetCurrentUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        bool cancelled = await handler.Handle(new CancelAttendance.Request(userId.Value, id), cancellationToken);
        return cancelled ? Results.NoContent() : Results.NotFound();
    }
}
