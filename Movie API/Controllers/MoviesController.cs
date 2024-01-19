using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_API.Dto;
using Movie_API.Models;

namespace Movie_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDBContext Context;

        private new List<string> AllowedExtensions = new List<string> { ".jpg" , ".png"};
        private long MaxAllowedPosterSize = 1048576 ;
        public MoviesController(ApplicationDBContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Movies = await Context.Movies
                .OrderByDescending(x => x.Rate)
                .Include(m => m.Genre)
                .Select(m => new MovieDetailsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    GenreId = m.GenreId,
                    Poster = m.Poster,
                    StoreLine = m.StoreLine,
                    Rate = m.Rate,
                    GenreName = m.Genre.Name
                })
                .ToListAsync();
            return Ok(Movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            //var Movie = await Context.Movies.FindAsync(id);
            var Movie = await Context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m=>m.Id == id);

            if (Movie == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var MovieDto = new MovieDetailsDto
            {
                Id = Movie.Id,
                Title = Movie.Title,
                Year = Movie.Year,
                GenreId = Movie.GenreId,
                Poster = Movie.Poster,
                StoreLine = Movie.StoreLine,
                Rate = Movie.Rate,
                GenreName = Movie.Genre.Name
            };


            return Ok(MovieDto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(int GenreId)
        {
            var Movies = await Context.Movies
               .Where(m=> m.GenreId ==GenreId)
               .OrderByDescending(x => x.Rate)
               .Include(m => m.Genre)
               .Select(m => new MovieDetailsDto
               {
                   Id = m.Id,
                   Title = m.Title,
                   Year = m.Year,
                   GenreId = m.GenreId,
                   Poster = m.Poster,
                   StoreLine = m.StoreLine,
                   Rate = m.Rate,
                   GenreName = m.Genre.Name
               })
               .ToListAsync();
            return Ok(Movies);

        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto MovieDto)
        {
            if(MovieDto.Poster == null)
            {
                return BadRequest("Poster is required!");
            }

            if (!AllowedExtensions.Contains(Path.GetExtension(MovieDto.Poster.FileName.ToLower())))
            {
                return BadRequest("Only allowed extentions is .png & .jpg");
            }

            if(MovieDto.Poster.Length > MaxAllowedPosterSize)
            {
                return BadRequest("Max length allowed is 1 MB");
            }

            var IsValidGenre = await Context.Genres.AnyAsync(g => g.Id == MovieDto.GenreId);
            if (!IsValidGenre)
            {
                return BadRequest("Invalid genre ID!");
            }

            using var DataStream = new MemoryStream();

            await MovieDto.Poster.CopyToAsync(DataStream);

            var Movie = new Movie 
            {
                GenreId = MovieDto.GenreId ,
                Title = MovieDto.Title ,
                Poster = DataStream.ToArray() ,
                Year = MovieDto.Year ,
                Rate = MovieDto.Rate ,
                StoreLine = MovieDto.StoreLine 
            };
            await Context.AddAsync(Movie);
            Context.SaveChanges();
            return Ok(Movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id ,[FromForm] MovieDto MovieDto)
        {
            var movie = await Context.Movies.FindAsync(id);
            if (movie == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var IsValidGenre = await Context.Genres.AnyAsync(g => g.Id == MovieDto.GenreId);

            if (!IsValidGenre)
            {
                return BadRequest("Invalid genre ID!");
            }
            if(MovieDto.Poster != null)
            {
                if (!AllowedExtensions.Contains(Path.GetExtension(MovieDto.Poster.FileName.ToLower())))
                {
                    return BadRequest("Only allowed extentions is .png & .jpg");
                }

                if (MovieDto.Poster.Length > MaxAllowedPosterSize)
                {
                    return BadRequest("Max length allowed is 1 MB");
                }

                using var DataStream = new MemoryStream();

                await MovieDto.Poster.CopyToAsync(DataStream);
                movie.Poster = DataStream.ToArray();
            }
            movie.Title= MovieDto.Title;
            movie.Year= MovieDto.Year;
            movie.Rate= MovieDto.Rate;
            movie.StoreLine = MovieDto.StoreLine;
            movie.GenreId = MovieDto.GenreId;

            Context.SaveChanges();
            return Ok(movie);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await Context.Movies.FindAsync(id);
            if(movie == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Context.Remove(movie);
            Context.SaveChanges();
            return Ok(movie);
        }
    }
}
