namespace MyFirstApi.DTOs;

public record BookSummaryResponse(
    string Title,
    string AuthorName,
    int PublishedYear);