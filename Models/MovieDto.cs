using System.ComponentModel.DataAnnotations;


namespace Lab6.Models;
public class MovieDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Tytuł jest wymagany")]
    [MaxLength(50)]
    public string Title { get; set; }
    [UIHint("LongText")]
    [Required(ErrorMessage = "Opis jest wymagany")]
    public string Description { get; set; }
    [UIHint("Stars")]
    [Range(1, 5, ErrorMessage = "Ocena filmu musi być liczbą pomiędzy 1 a 5")]
    public int Rating { get; set; }
    public string? TrailerLink { get; set; }
    public string Genre { get; set; }
    public List<string>? AllGenres { get; set; }
}