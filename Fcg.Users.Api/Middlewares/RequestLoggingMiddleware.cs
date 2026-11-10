namespace Fcg.Users.Api.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var inicio = DateTime.UtcNow;

        try
        {
            await _next(context);
        }
        finally
        {
            var duracao = DateTime.UtcNow - inicio;

            var descricaoStatus = context.Response.StatusCode switch
            {
                200 => "OK",
                201 => "Criado",
                400 => "Requisição inválida",
                401 => "Não autenticado",
                403 => "Acesso proibido",
                404 => "Não encontrado",
                500 => "Erro interno",
                _ => "Resposta HTTP"
            };

            _logger.LogInformation(
                "Requisição HTTP {Metodo} {Rota} | Status: {StatusCode} - {DescricaoStatus} | Duração: {DuracaoMs:F2} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                descricaoStatus,
                duracao.TotalMilliseconds);
        }
    }
}