using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using static Application.Common.Utilities;

namespace Application.Users;

public static class UpdateUserProfile
{
    public sealed record Request(
        Guid Id,
        UpdateField<string> FirstName = default,
        UpdateField<string> LastName = default,
        UpdateField<string?> Bio = default,
        UpdateField<string?> Industry = default,
        UpdateField<string?> Organization = default,
        UpdateField<string?> Title = default,
        UpdateField<string?> Country = default,
        UpdateField<string?> State = default,
        UpdateField<string?> City = default,
        UpdateField<string?> Interests = default,
        UpdateField<string?> Pronouns = default,
        UpdateField<string?> ProfileImage = default);

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

            if (request.FirstName.HasValue)
            {
                user.FirstName = NormalizeRequired(request.FirstName.Value, nameof(request.FirstName));
            }

            if (request.LastName.HasValue)
            {
                user.LastName = NormalizeRequired(request.LastName.Value, nameof(request.LastName));
            }

            if (request.Bio.HasValue)
            {
                user.Bio = Normalize(request.Bio.Value);
            }

            if (request.Industry.HasValue)
            {
                user.Industry = Normalize(request.Industry.Value);
            }

            if (request.Organization.HasValue)
            {
                user.Organization = Normalize(request.Organization.Value);
            }

            if (request.Title.HasValue)
            {
                user.Title = Normalize(request.Title.Value);
            }

            if (request.Country.HasValue)
            {
                user.Country = Normalize(request.Country.Value);
            }

            if (request.State.HasValue)
            {
                user.State = Normalize(request.State.Value);
            }

            if (request.City.HasValue)
            {
                user.City = Normalize(request.City.Value);
            }

            if (request.Interests.HasValue)
            {
                user.Interests = Normalize(request.Interests.Value);
            }

            if (request.Pronouns.HasValue)
            {
                user.Pronouns = Normalize(request.Pronouns.Value);
            }

            if (request.ProfileImage.HasValue)
            {
                user.ProfileImage = string.IsNullOrWhiteSpace(request.ProfileImage.Value) ? null : new Uri(request.ProfileImage.Value);
            }

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

}
