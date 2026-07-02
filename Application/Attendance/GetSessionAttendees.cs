using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Attendance;

public static class GetSessionAttendees
{
    public sealed record Request(Guid SessionId);

    public sealed record Response(
        Guid UserId,
        string? Email,
        string FirstName,
        string LastName,
        string Status,
        DateTime RegisteredAt,
        DateTime? CheckedInAt);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<IReadOnlyList<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await context.UserAttendings
                .AsNoTracking()
                .Where(x => x.SessionId == request.SessionId)
                .OrderBy(x => x.User.LastName)
                .ThenBy(x => x.User.FirstName)
                .Select(x => new Response(
                    x.UserId,
                    x.User.Email,
                    x.User.FirstName,
                    x.User.LastName,
                    x.Status.ToString(),
                    x.RegisteredAt,
                    x.CheckedInAt))
                .ToListAsync(cancellationToken);
        }
    }
}
