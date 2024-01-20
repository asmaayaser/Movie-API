using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_API.Dto;
using Movie_API.Models;
using Movie_API.Services;

namespace Movie_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService GenresService;
        public GenresController(IGenresService genresService)
        {
            GenresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Genres = await GenresService.GetAll();
            return Ok(Genres);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Genres = await GenresService.GetById(id);
            return Ok(Genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto GenreDto )
        {
            var Genre = new Genre { Name = GenreDto.Name };
            await GenresService.Add(Genre);
            return Ok(Genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync( int id , [FromBody] GenreDto GenreDto)
        {
            var Genra = await GenresService.GetById(id);
            if (Genra == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Genra.Name = GenreDto.Name;
            GenresService.Update(Genra);
            return Ok(Genra);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var Genra = await GenresService.GetById(id);
            if (Genra == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            GenresService.Delete(Genra);
            return Ok(Genra);

        }
    }
}
