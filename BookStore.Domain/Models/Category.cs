using BookStore.Domain.Common.Models;

namespace BookStore.Domain.Models;

public class Category : Entity<Category, int>
{
    
    #region Fields

    private string _categoryName;

    public string CategoryName
    {
        get => _categoryName;
        set => _categoryName = ValidateNullOrEmpty(value);
    }

    #endregion

    #region Properties

    public IEnumerable<Book> Books { get; set; } = default!;

    #endregion

    #region Constructors
    
    public Category(int id, string categoryName) : base(id)
    {
        _categoryName = categoryName;
    }

    public Category(string categoryName)
    {
        _categoryName = categoryName;
    }

    #endregion
    
}