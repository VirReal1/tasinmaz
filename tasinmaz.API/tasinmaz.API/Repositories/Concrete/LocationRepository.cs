using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tasinmaz.API.Entities.Abstract;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public class LocationRepository<T> : ILocationRepository<T> where T : class, IEntity, new()
    {
        DataContext _context;

        public LocationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(filter);
        }
        public async Task<ICollection<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<ICollection<T>> GetById(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);
        }

        public async Task<bool> AddAsync(T entity)
        {
            var addedEntity = await _context.Set<T>().AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.ChangeTracker.Clear();
            var updatedEntity = _context.Set<T>().Update(entity);
            return await SaveChanges();
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            var deletedEntity = _context.Set<T>().Remove(entity);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}