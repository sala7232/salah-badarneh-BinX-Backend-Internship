using LibraryDomain.Models;

namespace MyFirstApi.Data;

public static class BookStore
{
    private static readonly Author DefaultAuthor =
        new Author("Salah Badarneh", "Palestine");

    public static List<Book> Books { get; } = new()
    {
        new Book("Clean Code", DefaultAuthor, 2008),
        new Book("The Pragmatic Programmer", DefaultAuthor, 1999),
        new Book("C# in Depth", DefaultAuthor, 2019)
    };
}