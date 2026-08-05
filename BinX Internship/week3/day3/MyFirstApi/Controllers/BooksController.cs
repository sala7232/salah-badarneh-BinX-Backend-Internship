using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.DTOs;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers;

[ApiController]
[Route("api/v1/books")]
public class BooksController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public BooksController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll(
        [FromQuery] int? publishedYear)
    {
        if (publishedYear.HasValue &&
            (publishedYear < 1000 || publishedYear > 9999))
        {
            return BadRequest(new
            {
                message = "Published year must be between 1000 and 9999."
            });
        }

        IQueryable<Book> query = _context.Books.AsNoTracking();

        if (publishedYear.HasValue)
        {
            query = query.Where(
                book => book.PublishedYear == publishedYear.Value);
        }

        var books = await query
            .OrderBy(book => book.Id)
            .Select(book => new BookResponse(
                book.Id,
                book.Title,
                book.PublishedYear,
                book.AuthorId,
                book.Author.Name))
            .ToListAsync();

        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponse>> GetById(int id)
    {
        var book = await _context.Books
            .AsNoTracking()
            .Where(book => book.Id == id)
            .Select(book => new BookResponse(
                book.Id,
                book.Title,
                book.PublishedYear,
                book.AuthorId,
                book.Author.Name))
            .FirstOrDefaultAsync();

        if (book is null)
        {
            return NotFound(new
            {
                message = $"Book with ID {id} was not found."
            });
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> Create(
        CreateBookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new
            {
                message = "Title cannot be empty."
            });
        }

        var author = await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(
                author => author.Id == request.AuthorId);

        if (author is null)
        {
            return BadRequest(new
            {
                message = $"Author with ID {request.AuthorId} does not exist."
            });
        }

        var book = new Book
        {
            Title = request.Title.Trim(),
            PublishedYear = request.PublishedYear,
            AuthorId = request.AuthorId
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var response = new BookResponse(
            book.Id,
            book.Title,
            book.PublishedYear,
            book.AuthorId,
            author.Name);

        return CreatedAtAction(
            nameof(GetById),
            new { id = book.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookResponse>> Update(
        int id,
        UpdateBookRequest request)
    {
        var book = await _context.Books.FindAsync(id);

        if (book is null)
        {
            return NotFound(new
            {
                message = $"Book with ID {id} was not found."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new
            {
                message = "Title cannot be empty."
            });
        }

        var author = await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(
                author => author.Id == request.AuthorId);

        if (author is null)
        {
            return BadRequest(new
            {
                message = $"Author with ID {request.AuthorId} does not exist."
            });
        }

        book.Title = request.Title.Trim();
        book.PublishedYear = request.PublishedYear;
        book.AuthorId = request.AuthorId;

        await _context.SaveChangesAsync();

        var response = new BookResponse(
            book.Id,
            book.Title,
            book.PublishedYear,
            book.AuthorId,
            author.Name);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book is null)
        {
            return NotFound(new
            {
                message = $"Book with ID {id} was not found."
            });
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}