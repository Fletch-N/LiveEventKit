using Application.Attendance;
using Application.Events;
using Application.Following;
using Application.Sessions;
using Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateEvent.Handler>();
        services.AddScoped<CreateSession.Handler>();
        services.AddScoped<CancelAttendance.Handler>();
        services.AddScoped<DeleteSession.Handler>();
        services.AddScoped<DeleteEvent.Handler>();
        services.AddScoped<FollowUser.Handler>();
        services.AddScoped<GetSession.Handler>();
        services.AddScoped<GetSessionAttendees.Handler>();
        services.AddScoped<GetEvent.Handler>();
        services.AddScoped<GetUserAttendances.Handler>();
        services.AddScoped<GetUserProfile.Handler>();
        services.AddScoped<ListFollowers.Handler>();
        services.AddScoped<ListEvents.Handler>();
        services.AddScoped<ListFollowing.Handler>();
        services.AddScoped<ListSessions.Handler>();
        services.AddScoped<ListUsers.Handler>();
        services.AddScoped<RegisterAttendance.Handler>();
        services.AddScoped<UpdateEvent.Handler>();
        services.AddScoped<UpdateSession.Handler>();
        services.AddScoped<UpdateUserProfile.Handler>();
        services.AddScoped<UnfollowUser.Handler>();

        return services;
    }
}
