using System.Text;

namespace BookStore.Domain.Common.Exceptions;

public class PropertyNullException : Exception, IValidationError
{
    public PropertyNullException(string className, string propertyName)
    {
        Error = BuildMessage(className, propertyName);
    }

    private static string BuildMessage(string className, string propertyName)
    {
        var sb = new StringBuilder();
        sb.Append("Property ");
        sb.Append(propertyName);
        sb.Append(" From class ");
        sb.Append(className);
        sb.Append(" is required");

        return sb.ToString();
    }

    public string Error { get; }
}