using Microsoft.AspNetCore.Identity;
namespace Domain;

public enum UserRoles
{
    Admin,
    Staff,
    Speaker,
    Attendee,
    Guest
}
public class KitUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Industry { get; set; }
    public string? Organization { get; set; }
    public string? Title { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public string? Interests { get; set; }
    public string? Pronouns { get; set; }
    public Uri? ProfileImage { get; set; }

    public virtual ICollection<UserAttending> Attending { get; set; } = [];
    public virtual ICollection<UserFollowing> Following { get; set; } = [];
    public virtual ICollection<UserFollowing> Followers { get; set; } = [];
}
