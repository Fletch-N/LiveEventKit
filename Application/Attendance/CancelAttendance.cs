using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Attendance;

public static class CancelAttendance
{
    public sealed record Request(Guid UserId, Guid SessionId);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            UserAttending? entity = await context.UserAttendings
                .FirstOrDefaultAsync(
                    x => x.UserId == request.UserId && x.SessionId == request.SessionId,
                    cancellationToken);

            if (entity is null)
            {
                return false;
            }

            entity.Status = AttendingStatus.Cancelled;
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
