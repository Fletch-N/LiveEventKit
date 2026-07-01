using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Sessions;

public static class UpdateSession
{
    public sealed record Request(
        Guid Id,
        Guid EventId,
        Guid SpeakerId,
        string Title,
        string Description,
        string Category,
        string? Sponsor,
        DateTimeOffset StartTime,
        TimeSpan Duration,
        SessionAccessLevel AccessLevel,
        string? Image);

    public sealed record Response(
        Guid Id,
        Guid EventId,
        Guid SpeakerId,
        string Title,
        string Description,
        string Category,
        string? Sponsor,
        DateTimeOffset StartTime,
        TimeSpan Duration,
        string AccessLevel,
        string? Image);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<Response?> Handle(Request request, CancellationToken cancellationToken)
        {
            ValidateDuration(request.Duration);

            KitSession? entity = await context.Sessions
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return null;
            }

            entity.EventId = request.EventId;
            entity.SpeakerId = request.SpeakerId;
            entity.Title = request.Title.Trim();
            entity.Description = request.Description.Trim();
            entity.Category = request.Category.Trim();
            entity.Sponsor = string.IsNullOrWhiteSpace(request.Sponsor) ? null : request.Sponsor.Trim();
            entity.StartTime = request.StartTime;
            entity.Duration = request.Duration;
            entity.AccessLevel = request.AccessLevel;
            entity.Image = string.IsNullOrWhiteSpace(request.Image) ? null : new Uri(request.Image);
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return new Response(
                entity.Id,
                entity.EventId,
                entity.SpeakerId,
                entity.Title,
                entity.Description,
                entity.Category,
                entity.Sponsor,
                entity.StartTime,
                entity.Duration,
                entity.AccessLevel.ToString(),
                entity.Image?.ToString());
        }
    }

    private static void ValidateDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }
    }
}
