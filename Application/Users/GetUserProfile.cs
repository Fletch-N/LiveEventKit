using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Users;

public static class GetUserProfile
{
    public sealed record Request(Guid Id);

    public sealed record Response(
        Guid Id,
        string? Email,
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
            return await context.Users
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new Response(
                    x.Id,
                    x.Email,
                    x.FirstName,
                    x.LastName,
                    x.Bio,
                    x.Industry,
                    x.Organization,
                    x.Title,
                    x.Country,
                    x.State,
                    x.City,
                    x.Interests,
                    x.Pronouns,
                    x.ProfileImage == null ? null : x.ProfileImage.ToString()))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
