namespace Domain;

public class KitEvent : Auditable
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Uri? Image { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public virtual ICollection<KitSession> RunOfShow { get; set; } = [];
}
