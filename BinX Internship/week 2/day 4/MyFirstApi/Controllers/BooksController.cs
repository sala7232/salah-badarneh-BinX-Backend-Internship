using LibraryDomain.Models;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Data;

namespace MyFirstApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(BookStore.Books);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = BookStore.Books.FirstOrDefault(book => book.PublishedYear == id);

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }
}