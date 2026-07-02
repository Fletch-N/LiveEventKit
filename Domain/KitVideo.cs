namespace Domain;

public enum LiveStreamStatus
{
    Upcoming,
    Live,
    Paused,
    Ended,
    Cancelled,
    Failed
}

public class KitVideo : Auditable
{
    public Guid SessionId { get; set; }
    public KitSession Session { get; set; } = null!;
    public required Uri PlaybackUrl { get; set; }
    public LiveStreamStatus Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public TimeSpan? TotalDuration { get; set; }
}
