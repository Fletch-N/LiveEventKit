using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Following;

public static class FollowUser
{
    public sealed record Request(Guid ObserverId, Guid TargetId);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            if (request.ObserverId == request.TargetId)
            {
                throw new ArgumentException("A user cannot follow themselves.");
            }

            bool exists = await context.UserFollowings
                .AnyAsync(
                    x => x.ObserverId == request.ObserverId && x.TargetId == request.TargetId,
                    cancellationToken);

            if (exists)
            {
                return false;
            }

            context.UserFollowings.Add(new UserFollowing
            {
                ObserverId = request.ObserverId,
                TargetId = request.TargetId,
                FollowedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
