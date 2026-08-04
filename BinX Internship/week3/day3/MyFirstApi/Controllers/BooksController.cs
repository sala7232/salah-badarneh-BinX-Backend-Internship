using Microsoft.AspNetCore.Mvc;
using MyFirstApi.DTOs;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookSummaryService _bookSummaryService;

    public BooksController(IBookSummaryService bookSummaryService)
    {
        _bookSummaryService = bookSummaryService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BookSummaryResponse>> GetAll()
    {
        return Ok(_bookSummaryService.GetAll());
    }

    [HttpGet("{publishedYear:int}")]
    public ActionResult<BookSummaryResponse> GetByPublishedYear(
        int publishedYear)
    {
        var book = _bookSummaryService.GetByPublishedYear(publishedYear);

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }
}