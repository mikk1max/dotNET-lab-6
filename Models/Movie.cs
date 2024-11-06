using System.ComponentModel.DataAnnotations;


namespace Lab6.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł filmu jest wymagany")]
        [MaxLength(50, ErrorMessage = "Tytuł filmu nie może być dłuższy niż 50 znaków")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Opis filmu jest wymagany")]
        [UIHint("LongText")]
        public string Description { get; set; }

        [Range(1, 5, ErrorMessage = "Ocena filmu musi być liczbą pomiędzy 1 a 5")]
        [UIHint("Stars")]
        public int Rating { get; set; }
        public Genre Genre { get; set; }
        public string? TrailerLink { get; set; }
    }
}
