using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.DTOs;

public class CreateBookRequest
{
    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    [Range(1000, 9999)]
    public short PublishedYear { get; set; }

    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }
}