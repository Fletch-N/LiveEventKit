namespace Domain;

public enum SessionAccessLevel
{
    Public,
    Registered,
    VIP
}
public class KitSession : Auditable
{
    public Guid EventId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public Guid SpeakerId { get; set; }
    public KitUser Speaker { get; set; } = null!;
    public string? Sponsor { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public SessionAccessLevel AccessLevel { get; set; }
    public Uri? Image { get; set; }
    public KitVideo? Video { get; set; }
    public KitChat? Chat { get; set; } = null!;
    public virtual ICollection<UserAttending> Attendees { get; set; } = [];
}
