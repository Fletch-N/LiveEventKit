using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<KitUser> Users { get; }
    DbSet<KitEvent> Events { get; }
    DbSet<KitSession> Sessions { get; }
    DbSet<KitVideo> Videos { get; }
    DbSet<KitChat> Chats { get; }
    DbSet<ChatMessage> ChatMessages { get; }
    DbSet<ChatEvent> ChatEvents { get; }
    DbSet<ModerationEvent> ModerationEvents { get; }
    DbSet<ShowControl> ShowControls { get; }
    DbSet<UserAttending> UserAttendings { get; }
    DbSet<UserFollowing> UserFollowings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
