using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Following;

public static class UnfollowUser
{
    public sealed record Request(Guid ObserverId, Guid TargetId);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            Domain.UserFollowing? entity = await context.UserFollowings
                .FirstOrDefaultAsync(
                    x => x.ObserverId == request.ObserverId && x.TargetId == request.TargetId,
                    cancellationToken);

            if (entity is null)
            {
                return false;
            }

            context.UserFollowings.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
