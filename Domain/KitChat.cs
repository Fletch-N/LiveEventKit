namespace Domain;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public required string Body { get; set; }
    public bool IsHidden { get; set; }
    public Guid AuthorId { get; set; }
    public KitUser Author { get; set; } = null!;
    public DateTimeOffset TimeStamp { get; set; }
}

public enum ChatEventType
{
    Joined,
    Left,
    Disconnected,
}
public class ChatEvent
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public KitUser User { get; set; } = null!;
    public ChatEventType Type { get; set; }
    public DateTimeOffset TimeStamp { get; set; }

}


public enum ModerationCommand
{
    Mute,
    Unmute,
    Kick,
    Ban
}

public class ModerationEvent
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid TargetId { get; set; }
    public ModerationCommand Command { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}

public enum ShowControlCommand
{
    Advance,
    ShowAd,
    HideAd,
}

public class ShowControl
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public ShowControlCommand Command { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}

public class KitChat
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public KitSession Session { get; set; } = null!;
    public virtual ICollection<ChatMessage> Messages { get; set; } = [];
    public virtual ICollection<ChatEvent> ChatEvents { get; set; } = [];
    public virtual ICollection<ModerationEvent> ModerationEvents { get; set; } = [];
    public virtual ICollection<ShowControl> ShowControls { get; set; } = [];

}
