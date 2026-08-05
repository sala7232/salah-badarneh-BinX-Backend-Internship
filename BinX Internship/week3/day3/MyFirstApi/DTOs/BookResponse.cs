namespace MyFirstApi.DTOs;

public record BookResponse(
    int Id,
    string Title,
    short PublishedYear,
    int AuthorId,
    string AuthorName);