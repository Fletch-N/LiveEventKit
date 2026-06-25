using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Events;

public static class ListEvents
{
    public sealed record Request;

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
                .OrderBy(x => x.StartDate)
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
