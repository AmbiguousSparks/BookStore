namespace BookStore.API.Middlewares;

public class TaskCanceledExceptionMiddleware : IMiddleware
{
    private readonly ILogger<TaskCanceledExceptionMiddleware> _logger;

    public TaskCanceledExceptionMiddleware(ILogger<TaskCanceledExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while handling {Name}, message: {Message}", next.Method.Name, e.Message);
            throw new Exception("An error occured, please try again later.");
        }
    }
}