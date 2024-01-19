using Movie_API.Models;
using System.ComponentModel.DataAnnotations;

namespace Movie_API.Dto
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        public string StoreLine { get; set; }

        public byte[] Poster { get; set; }

        public int GenreId { get; set; }

        public String GenreName { get; set; }
    }
}
