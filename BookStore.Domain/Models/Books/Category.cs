using BookStore.Domain.Common.Models;

namespace BookStore.Domain.Models.Books;

public class Category : Entity<Category, int>
{
    
    #region Fields

    private string _categoryName = default!;

    public string CategoryName
    {
        get => _categoryName;
        set => _categoryName = ValidateNullOrEmpty(value);
    }

    #endregion

    #region Constructors

    private Category()
    {
    }
    
    public Category(int id, string categoryName) : base(id)
    {
        _categoryName = categoryName;
    }

    public Category(string categoryName)
        : this()
    {
        CategoryName = categoryName;
    }

    #endregion
    
}