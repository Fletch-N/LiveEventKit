using static Application.Common.Utilities;
using Application.Common.Interfaces;
using Domain;

namespace Application.Sessions;

public static class CreateSession
{
    public sealed record Request(
        Guid EventId,
        Guid SpeakerId,
        string Title,
        string Description,
        string Category,
        string? Sponsor,
        DateTime StartTime,
        TimeSpan Duration,
        SessionAccessLevel AccessLevel,
        string? Image);

    public sealed record Response(Guid Id);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            ValidateDuration(request.Duration);

            KitSession entity = new()
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                SpeakerId = request.SpeakerId,
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                Category = request.Category.Trim(),
                Sponsor = string.IsNullOrWhiteSpace(request.Sponsor) ? null : request.Sponsor.Trim(),
                StartTime = request.StartTime,
                Duration = request.Duration,
                AccessLevel = request.AccessLevel,
                Image = string.IsNullOrWhiteSpace(request.Image) ? null : new Uri(request.Image),
                CreatedAt = DateTime.UtcNow
            };

            context.Sessions.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            return new Response(entity.Id);
        }
    }
}
