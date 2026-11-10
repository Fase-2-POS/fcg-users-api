using Fcg.Users.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Fcg.Users.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string Nome { get; set; } = string.Empty;

    public PerfilUsuario Perfil { get; set; }
        = PerfilUsuario.Usuario;
}