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
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? await _context.Set<T>().ToListAsync() : await _context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.ChangeTracker.Clear();
            _context.Set<T>().Update(entity);
            return await SaveChanges();
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            var entity = _context.Set<T>().FirstOrDefault(filter);
            _context.Set<T>().Remove(entity);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}