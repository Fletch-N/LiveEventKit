using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Sessions;

public static class ListSessions
{
    public sealed record Request(Guid? EventId = null);

    public sealed record Response(
        Guid Id,
        Guid EventId,
        Guid SpeakerId,
        string Title,
        string Description,
        string Category,
        string? Sponsor,
        DateTime StartTime,
        TimeSpan Duration,
        string AccessLevel,
        string? Image);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<IReadOnlyList<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.KitSession> query = context.Sessions.AsNoTracking();

            if (request.EventId.HasValue)
            {
                query = query.Where(x => x.EventId == request.EventId.Value);
            }

            return await query
                .OrderBy(x => x.StartTime)
                .Select(x => new Response(
                    x.Id,
                    x.EventId,
                    x.SpeakerId,
                    x.Title,
                    x.Description,
                    x.Category,
                    x.Sponsor,
                    x.StartTime,
                    x.Duration,
                    x.AccessLevel.ToString(),
                    x.Image == null ? null : x.Image.ToString()))
                .ToListAsync(cancellationToken);
        }
    }
}
