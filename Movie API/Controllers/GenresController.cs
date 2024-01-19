using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_API.Dto;
using Movie_API.Models;

namespace Movie_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDBContext Context;
        public GenresController(ApplicationDBContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Genres = await Context.Genres.OrderBy(G=> G.Name).ToListAsync();
            return Ok(Genres);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto GenreDto )
        {
            var Genre = new Genre { Name = GenreDto.Name };
            await Context.Genres.AddAsync(Genre);
            Context.SaveChanges();
            return Ok(Genre);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync( int id , [FromBody] GenreDto GenreDto)
        {
            var Genra = await Context.Genres.FirstOrDefaultAsync(G => G.Id == id);
            if(Genra == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Genra.Name = GenreDto.Name;
            Context.SaveChanges();
            return Ok(Genra);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var Genra = await Context.Genres.FirstOrDefaultAsync(G => G.Id == id);
            if (Genra == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Context.Remove(Genra);
            Context.SaveChanges();
            return Ok(Genra);

        }
    }
}
