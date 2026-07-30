using LibraryDomain.Models;
using MyFirstApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/minimal/books", () =>
{
    return Results.Ok(BookStore.Books);
});

app.MapGet("/minimal/books/{year:int}", (int year) =>
{
    Book? book = BookStore.Books
        .FirstOrDefault(book => book.PublishedYear == year);

    return book is null
        ? Results.NotFound()
        : Results.Ok(book);
});

app.Run();