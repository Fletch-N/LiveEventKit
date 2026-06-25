using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Events;

public static class DeleteEvent
{
    public sealed record Request(Guid Id);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            Domain.KitEvent? entity = await context.Events
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return false;
            }

            context.Events.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
