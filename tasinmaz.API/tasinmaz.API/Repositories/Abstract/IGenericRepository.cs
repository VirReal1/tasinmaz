using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tasinmaz.API.Entities.Abstract;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public interface IGenericRepository<T> where T : class, IEntity, new()
    {
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<bool> Exists(Expression<Func<T, bool>> filter);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}