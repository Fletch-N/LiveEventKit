using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Sessions;

public static class DeleteSession
{
    public sealed record Request(Guid Id);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            Domain.KitSession? entity = await context.Sessions
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return false;
            }

            context.Sessions.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
