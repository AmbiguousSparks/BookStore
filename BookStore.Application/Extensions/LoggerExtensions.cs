using Microsoft.Extensions.Logging;

namespace BookStore.Application.Extensions;

public static class LoggerExtensions
{
    public static void RequestStartedLog<TRequest>(this ILogger logger, string param1, TRequest param2)
        => BuildLogMessageTwoParams("Start handling {param1}, {param2}", new EventId(100, "Started"))(logger, param1,
            param2!, default!);

    public static void RequestFinishedLog<TRequest>(this ILogger logger, string param1, TRequest param2)
        => BuildLogMessageTwoParams("Finished handling {param1}, {param2}", new EventId(101, "Finished"))(logger,
            param1,
            param2!, default!);


    private static Action<ILogger, string, object, Exception> BuildLogMessageTwoParams(string message, EventId @event)
    {
        return LoggerMessage
            .Define<string, object>(LogLevel.Information, @event, message);
    }
}