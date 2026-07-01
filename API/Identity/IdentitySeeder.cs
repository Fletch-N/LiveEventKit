using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Identity;

public sealed class IdentitySeeder(
    RoleManager<IdentityRole<Guid>> roleManager,
    UserManager<KitUser> userManager,
    IOptions<IdentitySeedOptions> options)
{
    public async Task SeedAsync()
    {
        foreach (string roleName in Enum.GetNames<UserRoles>())
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to create role '{roleName}': {string.Join(", ", roleResult.Errors.Select(x => x.Description))}");
                }
            }
        }

        IdentitySeedOptions seedOptions = options.Value;
        if (string.IsNullOrWhiteSpace(seedOptions.AdminEmail) ||
            string.IsNullOrWhiteSpace(seedOptions.AdminPassword))
        {
            return;
        }

        KitUser? existingUser = await userManager.FindByEmailAsync(seedOptions.AdminEmail);
        if (existingUser is null)
        {
            KitUser adminUser = new()
            {
                Id = Guid.NewGuid(),
                UserName = seedOptions.AdminEmail,
                Email = seedOptions.AdminEmail,
                FirstName = seedOptions.AdminFirstName?.Trim() ?? "Admin",
                LastName = seedOptions.AdminLastName?.Trim() ?? "User",
                EmailConfirmed = true
            };

            IdentityResult createResult = await userManager.CreateAsync(adminUser, seedOptions.AdminPassword);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create seeded admin user: {string.Join(", ", createResult.Errors.Select(x => x.Description))}");
            }

            existingUser = adminUser;
        }

        foreach (string roleName in new[] { UserRoles.Admin.ToString(), UserRoles.Staff.ToString() })
        {
            if (!await userManager.IsInRoleAsync(existingUser, roleName))
            {
                IdentityResult addRoleResult = await userManager.AddToRoleAsync(existingUser, roleName);
                if (!addRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to assign role '{roleName}' to seeded admin user: {string.Join(", ", addRoleResult.Errors.Select(x => x.Description))}");
                }
            }
        }
    }
}
