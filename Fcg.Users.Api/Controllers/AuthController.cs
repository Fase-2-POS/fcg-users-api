using Fcg.Contracts;
using Fcg.Users.Api.DTOs.Auth;
using Fcg.Users.Infrastructure.Identity;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fcg.Users.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IPublishEndpoint publishEndpoint,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            Nome = request.Nome,
            Email = request.Email,
            UserName = request.Email,
            Perfil = Fcg.Users.Domain.Enums.PerfilUsuario.Usuario
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Senha);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                Erros = result.Errors.Select(error => error.Description)
            });
        }

        var roleResult = await _userManager.AddToRoleAsync(
            user,
            "Usuario");

        if (!roleResult.Succeeded)
        {
            return BadRequest(new
            {
                Erros = roleResult.Errors.Select(error => error.Description)
            });
        }

        await _publishEndpoint.Publish(new UserCreatedEvent(
            user.Id,
            user.Nome,
            user.Email!));

        _logger.LogInformation(
            "Evento UserCreatedEvent publicado. UserId: {UserId} | E-mail: {Email}",
            user.Id,
            user.Email);

        return Ok(new
        {
            Mensagem = "Usuário cadastrado com sucesso."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(
            request.Email);

        if (user is null)
        {
            return Unauthorized(new
            {
                Mensagem = "E-mail ou senha inválidos."
            });
        }

        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            request.Senha);

        if (!passwordValid)
        {
            return Unauthorized(new
            {
                Mensagem = "E-mail ou senha inválidos."
            });
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtService.GenerateToken(
            user,
            roles);

        return Ok(new
        {
            Token = token
        });
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("perfil")]
    public async Task<IActionResult> Perfil()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            user.Id,
            user.Nome,
            user.Email,
            user.Perfil
        });
    }
}