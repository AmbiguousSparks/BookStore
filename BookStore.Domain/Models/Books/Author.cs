using BookStore.Domain.Common.Models;

namespace BookStore.Domain.Models.Books;

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

    #endregion

    #region Constructors

    private Author()
    {
    }
    
    public Author(int id, string name, DateOnly birthDate) : base(id)
    {
        Name = name;
        BirthDate = birthDate;
    }

    public Author(string name, DateOnly birthDate)
        : this()
    {
        Name = name;
        BirthDate = birthDate;
    }

    #endregion
}