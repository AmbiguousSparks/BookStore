using System.Text;

namespace BookStore.Domain.Common.Exceptions;

public class EntityAlreadyExistsException : DomainException
{
    private EntityAlreadyExistsException(string message) : base(message)
    {
    }

    public static EntityAlreadyExistsException Throw(string className) =>
        new(BuildMessage(className));
    
    
    private static string BuildMessage(string className)
    {
        var sb = new StringBuilder();
        sb.Append(className);
        sb.Append(" Already exists");
        return sb.ToString();
    }
}