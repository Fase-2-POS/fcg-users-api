using Fcg.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fcg.Users.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsuariosController(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public IActionResult GetUsuarios()
    {
        var usuarios = _userManager.Users
            .Select(user => new
            {
                user.Id,
                user.Nome,
                user.Email,
                user.Perfil
            })
            .ToList();

        return Ok(usuarios);
    }
}