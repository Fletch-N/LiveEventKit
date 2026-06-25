using Application.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateEvent.Handler>();
        services.AddScoped<DeleteEvent.Handler>();
        services.AddScoped<GetEvent.Handler>();
        services.AddScoped<ListEvents.Handler>();
        services.AddScoped<UpdateEvent.Handler>();

        return services;
    }
}
