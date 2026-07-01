using Application;
using API.Endpoints;
using API.Identity;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi()
    .AddApplication()
    .AddPersistence(builder.Configuration, builder.Environment.ContentRootPath);

builder.Services.Configure<IdentitySeedOptions>(
    builder.Configuration.GetSection(IdentitySeedOptions.SectionName));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("EventWriteAccess", policy =>
        policy.RequireRole(UserRoles.Admin.ToString(), UserRoles.Staff.ToString()));

builder.Services.AddIdentityApiEndpoints<KitUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddScoped<IdentitySeeder>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (IServiceScope scope = app.Services.CreateScope())
{
    DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();

    IdentitySeeder seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
    await seeder.SeedAsync();
}

app.MapGroup("/api/auth")
    .WithTags("Authentication")
    .MapAuthEndpoints();

app.MapGroup("/api/events")
    .WithTags("Events")
    .MapEventEndpoints();

app.MapGroup("/api/sessions")
    .WithTags("Sessions")
    .MapSessionEndpoints();

app.MapGroup("/api/users")
    .WithTags("Users")
    .MapUserEndpoints();

app.Run();
