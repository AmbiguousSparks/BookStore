using System.Text;

namespace BookStore.Domain.Common.Exceptions;

public class PropertyNullException : DomainException
{
    private PropertyNullException(string message)
        : base(message)
    {
    }

    public static PropertyNullException Throw(string className, string propertyName) =>
        new(BuildMessage(className, propertyName));

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
}