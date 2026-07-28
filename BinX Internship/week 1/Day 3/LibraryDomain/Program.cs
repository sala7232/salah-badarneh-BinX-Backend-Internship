
using System.Net;
using System.Runtime.CompilerServices;
using LibraryDomain.Models;

RunLibraryDemo();

static void RunLibraryDemo()
{
    var author = new Author("salah badarneh", "palstine");
    var book = new Book("Clean Code", author, 2008);

    Console.WriteLine($"{book.Title} by {book.Author.Name} ({book.PublishedYear})");

    var summary= new BookSummaryDto(book.Title, book.Author.Name, book.PublishedYear);
    Console.WriteLine($"Summary: {summary}");

    Console.WriteLine();
    Console.WriteLine("sending notifications");

    INotifiable[] recipients = {book, author};
    foreach(var recipient in recipients)
    {
        recipient.Notify("New library update available");
    }
    
}
namespace LibraryDomain.Models
{
public class Author : INotifiable
{
    public String Name {get; private set;}
    public string Country {get; private set;}

    public Author(string name, string country)
    {
        if(string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Name cant be empty",nameof(name));

        Name = name;
        Country = country;
    }

    public void Notify(string message)
    {
        Console.WriteLine($"Email to {Name}: {message}");

    }
}

public class Book : INotifiable
{
    public string Title {get; private set;}
    public Author Author {get; private set;}
    public int PublishedYear { get; private set;}

    public Book(string title, Author author, int publishedYear)
    {
        if(string.IsNullOrWhiteSpace(title))
        throw new ArgumentException("Title cant be empty", nameof(title));

        Title = title;
        Author = author ?? throw new ArgumentNullException(nameof(author));
        PublishedYear = publishedYear;
    }

    public void Notify(string message)
    {
        Console.WriteLine($"Library log for '{Title}': {message}");

    }


}

record BookSummaryDto(string Title, string AuthorName, int PublishedYear);

public interface INotifiable
{
    void Notify(string message);
}
}