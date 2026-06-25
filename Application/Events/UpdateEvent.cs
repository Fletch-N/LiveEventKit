using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Events;

public static class UpdateEvent
{
    public sealed record Request(
        Guid Id,
        string Title,
        string Description,
        string? Image,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate);

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
            ValidateDates(request.StartDate, request.EndDate);

            Domain.KitEvent? entity = await context.Events
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return null;
            }

            entity.Title = request.Title.Trim();
            entity.Description = request.Description.Trim();
            entity.Image = string.IsNullOrWhiteSpace(request.Image) ? null : new Uri(request.Image);
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
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

    private static void ValidateDates(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("EndDate must be greater than or equal to StartDate.");
        }
    }
}
