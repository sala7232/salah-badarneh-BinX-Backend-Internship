


using System.Collections.Concurrent;
using System.Security.Claims;

var books = CreateSampleBooks();

ShowProgrammingBooks(books);
ShowTitle(books);
ShowAverageYear(books);

await CheckAvailability("clean code");

ParseYear("2008");
ParseYear("not a year");

static List<Book> CreateSampleBooks() => new()
{
    new Book("clean code", "Robert Martin", 2008, "Programming"),
    new Book("The Pragmatic Programmer", "Andrew Hunt", 1999, "Programming"),
    new Book("Design Patterns", "Erich Gamma", 1994, "Programming"),
    new Book("Refactoring", "Martin Fowler", 1999, "Programming"),
    new Book("Introduction to Algorithms", "Thomas H. Cormen", 2009, "Algorithms"),
    new Book("Code Complete", "Steve McConnell", 2004, "Programming"),
    new Book("Head First Java", "Kathy Sierra", 2005, "Programming"),
    new Book("Effective Java", "Joshua Bloch", 2018, "Programming")

};

static void ShowProgrammingBooks(List<Book> books)
{
    Console.WriteLine("Programming books:");
    foreach (var book in books.Where(b => b.Genre == "Programming"))
    Console.WriteLine($"- {book.Title} ({book.PublishedYear})");
}
static void ShowAverageYear(List<Book> books)
{
    var average = books.Average(b => b.PublishedYear);
    Console.WriteLine($"\nAverage published Year: {average:0}");
}

static void ShowTitle(List<Book> books)
{
    Console.WriteLine("All titles:");
    foreach (var title in books.Select(b => b.Title) )
    Console.WriteLine($"- {title}");
}
static async Task CheckAvailability(string title)
{
    Console.WriteLine($"\nCheckin availability for '{title}' ");
    await Task.Delay(1000);
    Console.WriteLine($"'{title}' is available");
}

static void ParseYear(string input)
{
    try
    {
        int year = int.Parse(input);
        Console.WriteLine($"\nParsed year: {year}");

    }
    catch(FormatException)
    {
        Console.WriteLine($"\n'{input}' is not a valid year");
    }
}



class Book
{
    public string Title { get;}
    public string AuthorName { get;}
    public int PublishedYear { get;}
    public string Genre { get;}

    public Book(string title, string authorname, int publishedYear, string genre)
    {
        Title= title;
        AuthorName= authorname;
        PublishedYear = publishedYear;
        Genre= genre;
    }
}