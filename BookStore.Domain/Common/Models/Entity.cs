using System.Runtime.CompilerServices;
using BookStore.Domain.Common.Exceptions;
using BookStore.Domain.Common.Models.Interfaces;

namespace BookStore.Domain.Common.Models;

public abstract class Entity<TClass, TId> : IEntity<TId>
{
    public TId Id { get; } = default!;

    private readonly string _className;

    protected Entity(TId id) : this() =>
        Id = id;

    protected Entity()
    {
        _className = typeof(TClass).Name;
    }

    protected T ValidateNull<T>(T property, [CallerMemberName] string name = "") =>
        property ?? throw PropertyNullException.Throw(name, _className);

    protected string ValidateNullOrEmpty(string property, [CallerMemberName] string name = "") =>
        string.IsNullOrEmpty(property) ? throw PropertyNullException.Throw(name, _className) : property;
}