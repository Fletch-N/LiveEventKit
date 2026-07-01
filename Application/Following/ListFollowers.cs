using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Following;

public static class ListFollowers
{
    public sealed record Request(Guid UserId);

    public sealed record Response(Guid Id, string? Email, string FirstName, string LastName);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<IReadOnlyList<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await context.UserFollowings
                .AsNoTracking()
                .Where(x => x.TargetId == request.UserId)
                .OrderByDescending(x => x.FollowedAt)
                .Select(x => new Response(
                    x.Observer.Id,
                    x.Observer.Email,
                    x.Observer.FirstName,
                    x.Observer.LastName))
                .ToListAsync(cancellationToken);
        }
    }
}
