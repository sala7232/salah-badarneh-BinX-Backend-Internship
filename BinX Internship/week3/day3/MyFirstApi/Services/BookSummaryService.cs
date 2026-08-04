using MyFirstApi.Data;
using MyFirstApi.DTOs;

namespace MyFirstApi.Services;

public class BookSummaryService : IBookSummaryService
{
    public IEnumerable<BookSummaryResponse> GetAll()
    {
        return BookStore.Books.Select(book =>
            new BookSummaryResponse(
                book.Title,
                book.Author.Name,
                book.PublishedYear));
    }

    public BookSummaryResponse? GetByPublishedYear(int publishedYear)
    {
        var book = BookStore.Books.FirstOrDefault(
            book => book.PublishedYear == publishedYear);

        if (book is null)
        {
            return null;
        }

        return new BookSummaryResponse(
            book.Title,
            book.Author.Name,
            book.PublishedYear);
    }
}