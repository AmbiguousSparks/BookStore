namespace BookStore.Domain.Common.Models.Interfaces;

public interface IEntity<out TId>
{
    public TId Id { get; }
}