using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Following;

public static class ListFollowing
{
    public sealed record Request(Guid UserId);

    public sealed record Response(Guid Id, string? Email, string FirstName, string LastName);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<IReadOnlyList<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await context.UserFollowings
                .AsNoTracking()
                .Where(x => x.ObserverId == request.UserId)
                .OrderByDescending(x => x.FollowedAt)
                .Select(x => new Response(
                    x.Target.Id,
                    x.Target.Email,
                    x.Target.FirstName,
                    x.Target.LastName))
                .ToListAsync(cancellationToken);
        }
    }
}
