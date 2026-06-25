using Application.Events;

namespace API.Endpoints;

public static class EventEndpoints
{
    private sealed record UpdateEventBody(
        string Title,
        string Description,
        string? Image,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate);

    public static RouteGroupBuilder MapEventEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", ListEvents)
            .WithName("ListEvents");

        group.MapGet("/{id:guid}", GetEventById)
            .WithName("GetEventById");

        group.MapPost("/", CreateEvent)
            .WithName("CreateEvent");

        group.MapPut("/{id:guid}", UpdateEvent)
            .WithName("UpdateEvent");

        group.MapDelete("/{id:guid}", DeleteEvent)
            .WithName("DeleteEvent");

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
        UpdateEventBody body,
        UpdateEvent.Handler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            UpdateEvent.Response? result = await handler.Handle(
                new UpdateEvent.Request(
                    id,
                    body.Title,
                    body.Description,
                    body.Image,
                    body.StartDate,
                    body.EndDate),
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
