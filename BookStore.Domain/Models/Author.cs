using BookStore.Domain.Common.Models;

namespace BookStore.Domain.Models;

public class Author : Entity<Author, int>
{
    #region Fields

    private string _name = default!;

    public string Name
    {
        get => _name;
        set => _name = ValidateNullOrEmpty(value);
    }

    #endregion

    #region Properties

    public DateOnly BirthDate { get; set; } = default!;

    public IEnumerable<Book> Books { get; set; } = default!;

    #endregion

    #region Constructors

    public Author(int id, string name, DateOnly birthDate) : base(id)
    {
        Name = name;
        BirthDate = birthDate;
    }

    public Author(string name, DateOnly birthDate)
    {
        Name = name;
        BirthDate = birthDate;
    }

    #endregion
}