using System.Text;

namespace BookStore.Domain.Common.Exceptions;

public class EntityNotFound : IValidationError
{
    public string Error { get; }

    public EntityNotFound(string className)
    {
        Error = BuildMessage(className);
    }
    private static string BuildMessage(string className)
    {
        var sb = new StringBuilder();
        sb.Append(className);
        sb.Append(" Not found");
        return sb.ToString();
    }
}