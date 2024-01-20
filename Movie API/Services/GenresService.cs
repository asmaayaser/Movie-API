using Microsoft.EntityFrameworkCore;
using Movie_API.Dto;
using Movie_API.Models;

namespace Movie_API.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDBContext Context;
        public GenresService(ApplicationDBContext context)
        {
            Context = context;
        }
        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await Context.Genres.OrderBy(G => G.Name).ToListAsync();
        }
        public async Task<Genre> GetById(int id)
        {
            return await Context.Genres.FirstOrDefaultAsync(G => G.Id == id);
        }

        public async Task<Genre> Add(Genre genre)
        {
            await Context.Genres.AddAsync(genre);
            Context.SaveChanges();
            return genre;
        }

        public Genre Update(Genre genre)
        {
            Context.Update(genre);
            Context.SaveChanges();
            return genre;
        }
        public Genre Delete(Genre genre)
        {
            Context.Remove(genre);
            Context.SaveChanges();
            return genre;
        }


    }
}
