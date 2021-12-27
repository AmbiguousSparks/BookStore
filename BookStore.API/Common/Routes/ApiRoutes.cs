namespace BookStore.API.Common.Routes;

public static class ApiRoutes
{
    public static class Users
    {
        public const string GetAll = "api/Users";
        public const string Create = "api/Users";
        public const string Authenticate = "api/Users/Login";
        public const string GetPaged = "api/Users/Paged";
    }

    public static class AuthorsManagement
    {
        public const string Create = "api/AuthorsManagement";
    }
}