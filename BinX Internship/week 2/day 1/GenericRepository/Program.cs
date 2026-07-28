
using System.Net;
using LibraryDomain.Models;
var authorRepository = new Repository<Author>();
var bookRepository = new Repository<Book>();

var author= new Author("salah badarneh","Palestine");
var book = new Book("clean code",author ,2008);

authorRepository.Add(author);
bookRepository.Add(book);

var foundBook = bookRepository.Find(book => book.Title == "clean code");

if(foundBook is not null)
{
    Console.WriteLine($"found book: {foundBook.Title}");

}

IReadOnlyList<Book> books= bookRepository.GetAll();
foreach(var item in books)
{
    Console.WriteLine(item.Title);
}


//the class contstrait allows find to rutern null when no matching item is found
class Repository<T> where T : class
{

    private readonly List<T> _items = new();

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);

    }

    public  IReadOnlyList<T> GetAll()
    {
        return _items.AsReadOnly();
    }

    public T? Find(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _items.FirstOrDefault(predicate);
    }
}