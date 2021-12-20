using System.Text;

namespace BookStore.Domain.Common.Exceptions;

public class InvalidProperty : Exception, IValidationError
{
    public InvalidProperty(string className, string message)
    {
        Error = BuildMessage(className, message);
    }

    private static string BuildMessage(string className, string message)
    {
        var sb = new StringBuilder();
        sb.Append("Error message: ");
        sb.Append(message);
        sb.Append(" From class: ");
        sb.Append(className);

        return sb.ToString();
    }

    public string Error { get; }
}