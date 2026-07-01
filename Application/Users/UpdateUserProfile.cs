using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Users;

public static class UpdateUserProfile
{
    public sealed record Request(
        Guid Id,
        string FirstName,
        string LastName,
        string? Bio,
        string? Industry,
        string? Organization,
        string? Title,
        string? Country,
        string? State,
        string? City,
        string? Interests,
        string? Pronouns,
        string? ProfileImage);

    public sealed record Response(
        Guid Id,
        string FirstName,
        string LastName,
        string? Bio,
        string? Industry,
        string? Organization,
        string? Title,
        string? Country,
        string? State,
        string? City,
        string? Interests,
        string? Pronouns,
        string? ProfileImage);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<Response?> Handle(Request request, CancellationToken cancellationToken)
        {
            Domain.KitUser? user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user is null)
            {
                return null;
            }

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.Bio = Normalize(request.Bio);
            user.Industry = Normalize(request.Industry);
            user.Organization = Normalize(request.Organization);
            user.Title = Normalize(request.Title);
            user.Country = Normalize(request.Country);
            user.State = Normalize(request.State);
            user.City = Normalize(request.City);
            user.Interests = Normalize(request.Interests);
            user.Pronouns = Normalize(request.Pronouns);
            user.ProfileImage = string.IsNullOrWhiteSpace(request.ProfileImage) ? null : new Uri(request.ProfileImage);

            await context.SaveChangesAsync(cancellationToken);

            return new Response(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Bio,
                user.Industry,
                user.Organization,
                user.Title,
                user.Country,
                user.State,
                user.City,
                user.Interests,
                user.Pronouns,
                user.ProfileImage?.ToString());
        }
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
