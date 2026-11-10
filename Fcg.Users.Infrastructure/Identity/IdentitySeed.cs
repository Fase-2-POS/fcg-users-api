using Fcg.Users.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Fcg.Users.Infrastructure.Identity;

public static class IdentitySeed
{
    public static async Task SeedRolesAsync(
        RoleManager<IdentityRole<int>> roleManager)
    {
        string[] roles =
        [
            "Administrador",
            "Usuario"
        ];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole<int>(role));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Erro ao criar a role '{role}': " +
                        string.Join(", ",
                            result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    public static async Task SeedAdminAsync(
        UserManager<ApplicationUser> userManager)
    {
        const string email = "admin@fcg.com";
        const string senha = "Admin@123";

        var admin = await userManager.FindByEmailAsync(email);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                Nome = "Administrador",
                Email = email,
                UserName = email,
                Perfil = PerfilUsuario.Administrador,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                admin,
                senha);

            if (!result.Succeeded)
            {
                throw new Exception(
                    "Erro ao criar o administrador: " +
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(
                admin,
                "Administrador"))
        {
            var roleResult = await userManager.AddToRoleAsync(
                admin,
                "Administrador");

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    "Erro ao adicionar o administrador à role: " +
                    string.Join(", ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}