namespace Domain;

public enum AttendingStatus
{
    Registered,
    CheckedIn,
    Attended,
    Cancelled
}

public class UserAttending
{
    public Guid UserId { get; set; }
    public KitUser User { get; set; } = null!;

    public Guid SessionId { get; set; }
    public KitSession Session { get; set; } = null!;

    public DateTimeOffset RegisteredAt { get; set; }
    public DateTimeOffset? CheckedInAt { get; set; }
    public TimeSpan? TimeViewed { get; set; }

    public AttendingStatus Status { get; set; }
}
