using BookStore.Application.Common.Models;

namespace BookStore.Application.Common.Exceptions;

public class InvalidModelStateException : Exception
{
    public IEnumerable<PropertyError> Errors { get; } = new List<PropertyError>();
}