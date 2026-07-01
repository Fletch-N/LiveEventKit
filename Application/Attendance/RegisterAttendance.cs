using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Attendance;

public static class RegisterAttendance
{
    public sealed record Request(Guid UserId, Guid SessionId);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            UserAttending? existing = await context.UserAttendings
                .FirstOrDefaultAsync(
                    x => x.UserId == request.UserId && x.SessionId == request.SessionId,
                    cancellationToken);

            if (existing is not null)
            {
                existing.Status = AttendingStatus.Registered;
                existing.RegisteredAt = DateTimeOffset.UtcNow;
                existing.CheckedInAt = null;
                await context.SaveChangesAsync(cancellationToken);
                return false;
            }

            context.UserAttendings.Add(new UserAttending
            {
                UserId = request.UserId,
                SessionId = request.SessionId,
                RegisteredAt = DateTimeOffset.UtcNow,
                Status = AttendingStatus.Registered
            });

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
