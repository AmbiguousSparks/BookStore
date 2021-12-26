using BookStore.Domain.Common.Models;
using BookStore.Domain.Models.Enums;

namespace BookStore.Domain.Models.Books;

public class Book : Entity<int>
{
    #region Fields

    private string _title = default!;
    private string _description = default!;
    private string _isbn = default!;
    private Author _author = default!;
    private string _language = default!;
    private Category _category = default!;
    private decimal _price;
    private string _cover = default!;
    
    public Category Category
    {
        get => _category;
        set => _category = ValidateNull(value);
    }

    public Author Author
    {
        get => _author;
        set => _author = ValidateNull(value);
    }

    public string Title
    {
        get => _title;
        set => _title = ValidateNullOrEmpty(value);
    }

    public string Description
    {
        get => _description;
        set => _description = ValidateNullOrEmpty(value);
    }
    public string Isbn
    {
        get => _isbn;
        set => _isbn = ValidateNullOrEmpty(value);
    }
    public string Language
    {
        get => _language;
        set => _language = ValidateNullOrEmpty(value);
    }

    public decimal Price
    {
        get => _price;
        set => _price = ValidateZero(value);
    }
    
    public string Cover
    {
        get => _cover;
        set => _cover = ValidateNullOrEmpty(value);
    }

    #endregion

    #region Properties

    public DateOnly ReleaseDate { get; set; }

    public BookCoverType CoverType { get; set; }
    
    public int Edition { get; set; }


    #endregion

    #region Constructors

    private Book()
    {
    }

    public Book(int id, string title, string description, string isbn, Author author, string language, string cover, Category category, DateOnly releaseDate, BookCoverType coverType, int edition, decimal price) 
        : base(id)
    {
        Title = title;
        Description = description;
        Isbn = isbn;
        Author = author;
        Language = language;
        Category = category;
        ReleaseDate = releaseDate;
        CoverType = coverType;
        Edition = edition;
        Price = price;
        Cover = cover;
    }

    public Book(string title, string description, string isbn, Author author, string language, string cover, Category category, DateOnly releaseDate, BookCoverType coverType, int edition, decimal price)
        : this()
    {
        Title = title;
        Description = description;
        Isbn = isbn;
        Author = author;
        Language = language;
        Category = category;
        ReleaseDate = releaseDate;
        CoverType = coverType;
        Edition = edition;
        Price = price;
        Cover = cover;
    }

    #endregion
}