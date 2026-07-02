using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using static Application.Common.Utilities;

namespace Application.Users;

public static class UpdateUserProfile
{
    public sealed record Request
    {
        public Guid Id { get; init; }
        public UpdateField<string> FirstName { get; init; }
        public UpdateField<string> LastName { get; init; }
        public UpdateField<string?> Bio { get; init; }
        public UpdateField<string?> Industry { get; init; }
        public UpdateField<string?> Organization { get; init; }
        public UpdateField<string?> Title { get; init; }
        public UpdateField<string?> Country { get; init; }
        public UpdateField<string?> State { get; init; }
        public UpdateField<string?> City { get; init; }
        public UpdateField<string?> Interests { get; init; }
        public UpdateField<string?> Pronouns { get; init; }
        public UpdateField<string?> ProfileImage { get; init; }
    }

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
