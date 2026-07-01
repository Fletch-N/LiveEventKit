using Application.Events;

namespace API.Endpoints;

public static class EventEndpoints
{
    public static RouteGroupBuilder MapEventEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", ListEvents)
            .WithName("ListEvents");

        group.MapGet("/{id:guid}", GetEventById)
            .WithName("GetEventById");

        group.MapPost("/", CreateEvent)
            .WithName("CreateEvent")
            .RequireAuthorization("EventWriteAccess");

        group.MapPut("/{id:guid}", UpdateEvent)
            .WithName("UpdateEvent")
            .RequireAuthorization("EventWriteAccess");

        group.MapDelete("/{id:guid}", DeleteEvent)
            .WithName("DeleteEvent")
            .RequireAuthorization("EventWriteAccess");

        return group;
    }

    private static async Task<IResult> ListEvents(
        ListEvents.Handler handler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ListEvents.Response> result = await handler.Handle(
            new ListEvents.Request(),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetEventById(
        Guid id,
        GetEvent.Handler handler,
        CancellationToken cancellationToken)
    {
        GetEvent.Response? result = await handler.Handle(
            new GetEvent.Request(id),
            cancellationToken);

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    }

    private static async Task<IResult> CreateEvent(
        CreateEvent.Request request,
        CreateEvent.Handler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            CreateEvent.Response result = await handler.Handle(request, cancellationToken);
            return Results.Created($"/api/events/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> UpdateEvent(
        Guid id,
        UpdateEvent.Request request,
        UpdateEvent.Handler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            UpdateEvent.Response? result = await handler.Handle(
                request with { Id = id },
                cancellationToken);

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> DeleteEvent(
        Guid id,
        DeleteEvent.Handler handler,
        CancellationToken cancellationToken)
    {
        bool deleted = await handler.Handle(new DeleteEvent.Request(id), cancellationToken);

        return deleted
            ? Results.NoContent()
            : Results.NotFound();
    }
}
