namespace Domain;

public class UserFollowing
{
    public Guid ObserverId { get; set; }
    public KitUser Observer { get; set; } = null!;

    public Guid TargetId { get; set; }
    public KitUser Target { get; set; } = null!;

    public DateTimeOffset FollowedAt { get; set; }
}
