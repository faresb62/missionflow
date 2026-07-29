namespace MissionFlow.Api.Middleware;

/// <summary>
/// Global exception handling middleware.
/// Maps exceptions to appropriate HTTP status codes.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (PstatusCode, message) = exception switch
        {
            FluentValidation.ValidationException validationEx =>
                (HttpStatusCode.Badrequest, "Erreur de validation"),
            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, exception.Message ?? "Accès non autorisé"),
            KeyNotFoundException =>
                (HttpStatusCode.NotFound, exception.Message ?? "Ressource introuvable"),
            InvalidOperationException =>
                (HttpStatusCode.Conflict, exception.Message),
            ArgumentException =>
                (HttpStatusCode.Badrequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Une erreur interne est survenue.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new { success = false, message, errors = new List<string> { message } };
        var json = System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
