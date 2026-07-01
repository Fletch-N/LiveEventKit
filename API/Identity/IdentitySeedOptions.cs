namespace API.Identity;

public sealed class IdentitySeedOptions
{
    public const string SectionName = "IdentitySeed";

    public string? AdminEmail { get; init; }
    public string? AdminPassword { get; init; }
    public string? AdminFirstName { get; init; }
    public string? AdminLastName { get; init; }
}
