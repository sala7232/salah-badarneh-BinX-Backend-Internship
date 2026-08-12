namespace MyFirstApi.DTOs;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;

    public short PublishedYear { get; set; }

    public int AuthorId { get; set; }
}