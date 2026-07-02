using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Sessions;

public static class GetSession
{
    public sealed record Request(Guid Id);

    public sealed record Response(
        Guid Id,
        Guid EventId,
        Guid SpeakerId,
        string Title,
        string Description,
        string Category,
        string? Sponsor,
        DateTime StartTime,
        TimeSpan Duration,
        SessionAccessLevel AccessLevel,
        string? Image,
        Guid? VideoId,
        Guid? ChatId);

    public sealed class Handler(IApplicationDbContext context)
    {
        public async Task<Response?> Handle(Request request, CancellationToken cancellationToken)
        {
            return await context.Sessions
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new Response(
                    x.Id,
                    x.EventId,
                    x.SpeakerId,
                    x.Title,
                    x.Description,
                    x.Category,
                    x.Sponsor,
                    x.StartTime,
                    x.Duration,
                    x.AccessLevel,
                    x.Image == null ? null : x.Image.ToString(),
                    x.Video != null ? x.Video.Id : null,
                    x.Chat != null ? x.Chat.Id : null))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
