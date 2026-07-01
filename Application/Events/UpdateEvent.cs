using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using static Application.Common.Utilities;

namespace Application.Events;

public static class UpdateEvent
{
    public sealed record Request(
        Guid Id,
        UpdateField<string> Title = default,
        UpdateField<string> Description = default,
        UpdateField<string?> Image = default,
        UpdateField<DateTimeOffset> StartDate = default,
        UpdateField<DateTimeOffset> EndDate = default);

    public sealed record Response(
        Guid Id,
        string Title,
        string Description,
        string? Image,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<Response?> Handle(Request request, CancellationToken cancellationToken)
        {
            Domain.KitEvent? entity = await context.Events
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return null;
            }

            DateTimeOffset startDate = request.StartDate.HasValue ? request.StartDate.Value : entity.StartDate;
            DateTimeOffset endDate = request.EndDate.HasValue ? request.EndDate.Value : entity.EndDate;

            ValidateDates(startDate, endDate);

            if (request.Title.HasValue)
            {
                entity.Title = NormalizeRequired(request.Title.Value, nameof(request.Title));
            }

            if (request.Description.HasValue)
            {
                entity.Description = NormalizeRequired(request.Description.Value, nameof(request.Description));
            }

            if (request.Image.HasValue)
            {
                entity.Image = string.IsNullOrWhiteSpace(request.Image.Value) ? null : new Uri(request.Image.Value);
            }

            if (request.StartDate.HasValue)
            {
                entity.StartDate = request.StartDate.Value;
            }

            if (request.EndDate.HasValue)
            {
                entity.EndDate = request.EndDate.Value;
            }

            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return new Response(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.Image?.ToString(),
                entity.StartDate,
                entity.EndDate);
        }
    }
}
