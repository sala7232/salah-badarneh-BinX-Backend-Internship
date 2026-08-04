using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    [Range(1000, 9999)]
    public short PublishedYear { get; set; }

    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;
}