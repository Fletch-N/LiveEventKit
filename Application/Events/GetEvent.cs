using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Events;

public static class GetEvent
{
    public sealed record Request(Guid Id);

    public sealed record Response(
        Guid Id,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<Response?> Handle(Request request, CancellationToken cancellationToken)
        {
            return await context.Events
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new Response(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.StartDate,
                    x.EndDate))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
