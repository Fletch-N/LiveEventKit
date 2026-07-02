using static Application.Common.Utilities;
using Application.Common.Interfaces;
using Domain;

namespace Application.Events;

public static class CreateEvent
{
    public sealed record Request(
        string Title,
        string Description,
        string? Image,
        DateTime StartDate,
        DateTime EndDate);

    public sealed record Response(Guid Id);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            ValidateDates(request.StartDate, request.EndDate);

            KitEvent entity = new()
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                Image = string.IsNullOrWhiteSpace(request.Image) ? null : new Uri(request.Image),
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            context.Events.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            return new Response(entity.Id);
        }
    }
}
