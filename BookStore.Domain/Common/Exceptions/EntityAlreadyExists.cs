using System.Text;

namespace BookStore.Domain.Common.Exceptions;

public class EntityAlreadyExists : IValidationError
{
    public EntityAlreadyExists(string className)
    {
        Error = BuildMessage(className);
    }
    
    private static string BuildMessage(string className)
    {
        var sb = new StringBuilder();
        sb.Append(className);
        sb.Append(" Already exists");
        return sb.ToString();
    }

    public string Error { get; }
}