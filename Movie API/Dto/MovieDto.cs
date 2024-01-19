using System.ComponentModel.DataAnnotations;

namespace Movie_API.Dto
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        [MaxLength(2500)]
        public string StoreLine { get; set; }

        public IFormFile? Poster { get; set; }

        public int GenreId { get; set; }
    }
}
