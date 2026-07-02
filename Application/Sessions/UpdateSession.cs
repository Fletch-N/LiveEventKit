using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using Microsoft.EntityFrameworkCore;
using static Application.Common.Utilities;

namespace Application.Sessions;

public static class UpdateSession
{
    public sealed record Request
    {
        public Guid Id { get; init; }
        public UpdateField<Guid> EventId { get; init; }
        public UpdateField<Guid> SpeakerId { get; init; }
        public UpdateField<string> Title { get; init; }
        public UpdateField<string> Description { get; init; }
        public UpdateField<string> Category { get; init; }
        public UpdateField<string?> Sponsor { get; init; }
        public UpdateField<DateTimeOffset> StartTime { get; init; }
        public UpdateField<TimeSpan> Duration { get; init; }
        public UpdateField<SessionAccessLevel> AccessLevel { get; init; }
        public UpdateField<string?> Image { get; init; }
    }

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
            KitSession? entity = await context.Sessions
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return null;
            }

            TimeSpan duration = request.Duration.HasValue ? request.Duration.Value : entity.Duration;

            ValidateDuration(duration);

            if (request.EventId.HasValue)
            {
                entity.EventId = request.EventId.Value;
            }

            if (request.SpeakerId.HasValue)
            {
                entity.SpeakerId = request.SpeakerId.Value;
            }

            if (request.Title.HasValue)
            {
                entity.Title = NormalizeRequired(request.Title.Value, nameof(request.Title));
            }

            if (request.Description.HasValue)
            {
                entity.Description = NormalizeRequired(request.Description.Value, nameof(request.Description));
            }

            if (request.Category.HasValue)
            {
                entity.Category = NormalizeRequired(request.Category.Value, nameof(request.Category));
            }

            if (request.Sponsor.HasValue)
            {
                entity.Sponsor = Normalize(request.Sponsor.Value);
            }

            if (request.StartTime.HasValue)
            {
                entity.StartTime = request.StartTime.Value;
            }

            if (request.Duration.HasValue)
            {
                entity.Duration = request.Duration.Value;
            }

            if (request.AccessLevel.HasValue)
            {
                entity.AccessLevel = request.AccessLevel.Value;
            }

            if (request.Image.HasValue)
            {
                entity.Image = string.IsNullOrWhiteSpace(request.Image.Value) ? null : new Uri(request.Image.Value);
            }

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
}
