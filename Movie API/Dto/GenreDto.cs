using System.ComponentModel.DataAnnotations;

namespace Movie_API.Dto
{
    public class GenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
