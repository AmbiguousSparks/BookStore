using BookStore.Domain.Common.Models;
using BookStore.Domain.Models.Enums;

namespace BookStore.Domain.Models.Users;

public class User : Entity<User, int>
{
    #region Fields

    private string _firstName = default!;
    private string _lastName = default!;
    private string _email = default!;
    private string _password = default!;

    public string FirstName
    {
        get => _firstName;
        set => _firstName = ValidateNullOrEmpty(value);
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = ValidateNullOrEmpty(value);
    }

    public string Email
    {
        get => _email;
        set => _email = ValidateNullOrEmpty(value);
    }

    public string Password
    {
        get => _password;
        set => _password = ValidateNullOrEmpty(value);
    }

    #endregion

    #region Properties

    public UserType Type { get; set; }

    #endregion

    #region Constructors
    
    private User()
    {
    }

    public User(int id, string firstName, string lastName, string email, string password, UserType type) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Type = type;
    }

    public User(string firstName, string lastName, string email, string password, UserType type)
        : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Type = type;
    }

    #endregion
}