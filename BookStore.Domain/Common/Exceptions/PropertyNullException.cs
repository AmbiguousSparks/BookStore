using System.Text;

namespace BookStore.Domain.Common.Exceptions;

public class PropertyNullException : DomainException
{
    private PropertyNullException(string message)
        : base(message)
    {
    }

    public static PropertyNullException Throw(string className, string propertyName) =>
        new PropertyNullException(BuildMessage(className, propertyName));

    private static string BuildMessage(string className, string propertyName)
    {
        var sb = new StringBuilder();
        sb.Append("Propriedade ");
        sb.Append(propertyName);
        sb.Append("Da classe ");
        sb.Append(className);
        sb.Append("é obrigatória");

        return sb.ToString();
    }
}