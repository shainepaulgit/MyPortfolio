using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using MyPortfolio.Models.Repositories.Contracts;

namespace MyPortfolio.Models.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> 
        where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _table;
        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            _table = db.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _table.AddAsync(entity);
            await _db.SaveChangesAsync();   
        }

        public async Task Delete(string id)
        {

            var existingEntity = await GetOne(id);
            if (existingEntity != null)
            {
                _table.Remove(existingEntity);
               await  _db.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetAll()
        {
           return await _table.ToListAsync();
        }

        public async Task<T> GetOne(string id)
        {
            if (int.TryParse(id, out int parsedId))
                return await _table.FindAsync(parsedId);
            return await _table.FindAsync(id);
          
        }

        public async Task Update(T entity, string id)
        {
            var existingEntity = await GetOne(id);
            if (existingEntity != null)
            {
                _db.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
