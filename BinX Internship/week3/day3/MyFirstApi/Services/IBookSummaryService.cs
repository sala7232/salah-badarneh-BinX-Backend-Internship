using MyFirstApi.DTOs;

namespace MyFirstApi.Services;

public interface IBookSummaryService
{
    IEnumerable<BookSummaryResponse> GetAll();

    BookSummaryResponse? GetByPublishedYear(int publishedYear);
}