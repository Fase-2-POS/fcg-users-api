using System.Net;
using System.Text.Json;

namespace Fcg.Users.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Erro de validação. {Metodo} {Rota}",
                context.Request.Method,
                context.Request.Path);

            await ResponderErroAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro interno. {Metodo} {Rota}",
                context.Request.Method,
                context.Request.Path);

            await ResponderErroAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno no servidor.");
        }
    }

    private static async Task ResponderErroAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string mensagem)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var resposta = new
        {
            status = (int)statusCode,
            erro = mensagem,
            rota = context.Request.Path.ToString(),
            metodo = context.Request.Method
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(resposta));
    }
}