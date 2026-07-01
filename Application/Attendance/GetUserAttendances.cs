using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Attendance;

public static class GetUserAttendances
{
    public sealed record Request(Guid UserId);

    public sealed record Response(
        Guid SessionId,
        Guid EventId,
        string Title,
        string Status,
        DateTimeOffset StartTime,
        TimeSpan Duration,
        DateTimeOffset RegisteredAt,
        DateTimeOffset? CheckedInAt);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<IReadOnlyList<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await context.UserAttendings
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Session.StartTime)
                .Select(x => new Response(
                    x.SessionId,
                    x.Session.EventId,
                    x.Session.Title,
                    x.Status.ToString(),
                    x.Session.StartTime,
                    x.Session.Duration,
                    x.RegisteredAt,
                    x.CheckedInAt))
                .ToListAsync(cancellationToken);
        }
    }
}
