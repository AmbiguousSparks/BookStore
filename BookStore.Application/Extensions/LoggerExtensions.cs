using Microsoft.Extensions.Logging;

namespace BookStore.Application.Extensions;

public static class LoggerExtensions
{
    private static readonly EventId Event = new(100, "Started Handle");

    public static void RequestStartedLog<TRequest>(this ILogger logger, string param1, TRequest param2)
        => BuildLogMessageTwoParams("Start handling {param1}, {param2}")(logger, param1, param2!, default!);


    private static Action<ILogger, string, object, Exception> BuildLogMessageTwoParams(string message)
    {
        return LoggerMessage
            .Define<string, object>(LogLevel.Information, Event, message);
    }
}