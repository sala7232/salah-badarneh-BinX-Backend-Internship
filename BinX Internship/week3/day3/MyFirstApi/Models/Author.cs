using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models;

public class Author
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}