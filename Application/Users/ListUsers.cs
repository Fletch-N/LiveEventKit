using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Users;

public static class ListUsers
{
    public sealed record Request(
        string? Search = null,
        int Page = 0,
        int Limit = 25
    );

    public sealed record Response(
        Guid Id,
        string? Email,
        string FirstName,
        string LastName,
        string? Title,
        string? Organization,
        string? ProfileImage);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<IReadOnlyList<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await context.Users
                .AsNoTracking()
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit)
                .Select(x => new Response(
                    x.Id,
                    x.Email,
                    x.FirstName,
                    x.LastName,
                    x.Title,
                    x.Organization,
                    x.ProfileImage == null ? null : x.ProfileImage.ToString()))
                .ToListAsync(cancellationToken);
        }
    }
}
