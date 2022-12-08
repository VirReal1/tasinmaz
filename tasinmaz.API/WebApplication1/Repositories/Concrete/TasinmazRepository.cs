using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Models;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;
using TasinmazDto = tasinmaz.API.Dtos.Tasinmaz.TasinmazDto;

namespace tasinmaz.API.Data
{
    public class TasinmazRepository<T> : IGenericRepository<T> where T : TasinmazDto
    {
        DataContext _context;
        public TasinmazRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? await _context.Set<T>().ToListAsync() : await _context.Set<T>().Where(filter).ToListAsync();
        }
        public async Task<T> GetASync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? null : await _context.Set<T>().FirstOrDefaultAsync(filter);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);
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
            addedEntity.State = EntityState.Added;
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