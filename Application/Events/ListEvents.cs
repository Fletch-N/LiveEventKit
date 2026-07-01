using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Events;

public static class ListEvents
{
    public sealed record Request(
        string? Search = null,
        int Page = 0,
        int Limit = 25
    );

    public sealed record Response(
        Guid Id,
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<IReadOnlyList<Response>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            return await context.Events
                .AsNoTracking()
                .OrderBy(x => x.StartDate)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit)
                .Select(x => new Response(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.StartDate,
                    x.EndDate))
                .ToListAsync(cancellationToken);
        }
    }
}
