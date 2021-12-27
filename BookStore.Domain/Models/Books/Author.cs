using BookStore.Domain.Common.Models;

namespace BookStore.Domain.Models.Books;

public class Author : Entity<int>
{
    #region Fields

    private string _name = default!;
    private string _photo = default!;

    public string Name
    {
        get => _name;
        set => _name = ValidateNullOrEmpty(value);
    }

    public string Photo
    {
        get => _photo;
        set => _photo = ValidateNullOrEmpty(value);
    }

    #endregion

    #region Properties

    public DateTime BirthDate { get; set; }

    #endregion

    #region Constructors

    private Author()
    {
    }
    
    public Author(int id, string name, string photo, DateTime birthDate) : base(id)
    {
        Name = name;
        Photo = photo;
        BirthDate = birthDate;
    }

    public Author(string name, string photo, DateTime birthDate)
        : this()
    {
        Name = name;
        Photo = photo;
        BirthDate = birthDate;
    }

    #endregion
}