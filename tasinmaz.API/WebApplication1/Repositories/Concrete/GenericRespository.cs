using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using tasinmaz.API.Entities.Abstract;

namespace tasinmaz.API.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity, new()
    {
        DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? await _context.Set<T>().ToListAsync() : await _context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task<bool> AddAsync(T entity)
        {
            var addedEntity = await _context.Set<T>().AddAsync(entity);
            addedEntity.State = EntityState.Added;
            return await SaveChanges();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var addedEntity = _context.Set<T>().Update(entity);
            addedEntity.State = EntityState.Modified;
            return await SaveChanges();
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            var addedEntity = _context.Set<T>().Remove(entity);
            addedEntity.State = EntityState.Deleted;
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}